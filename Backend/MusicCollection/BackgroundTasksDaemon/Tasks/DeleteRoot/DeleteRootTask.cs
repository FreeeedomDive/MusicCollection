using MusicCollection.Api.Dto.Exceptions;
using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.BusinessLogic.Repositories.Files.Nodes;
using MusicCollection.BusinessLogic.Repositories.Files.Roots;
using MusicCollection.BusinessLogic.Repositories.Files.Tags;
using MusicCollection.Common.Loggers;

namespace BackgroundTasksDaemon.Tasks.DeleteRoot;

public class DeleteRootTask : IBackgroundTask
{
    public DeleteRootTask(
        IRootsRepository rootsRepository,
        INodesRepository nodesRepository,
        ITagsRepository tagsRepository,
        ILogger logger
    )
    {
        this.rootsRepository = rootsRepository;
        this.nodesRepository = nodesRepository;
        this.tagsRepository = tagsRepository;
        this.logger = logger;

        Id = new Guid();
        state = DeleteRootTaskState.Pending;
    }

    public async Task InitializeWithArgsAsync(string[]? args = null)
    {
        if (args is null || args.Length != 1)
        {
            throw new InvalidOperationException(
                $"{nameof(DeleteRootTask)} should have 1 arg - root id"
            );
        }

        if (!Guid.TryParse(args[0], out rootId))
        {
            throw new InvalidOperationException("First argument is not guid");
        }

        try
        {
            await rootsRepository.ReadAsync(rootId);
        }
        catch (RootNotFoundException)
        {
            state = DeleteRootTaskState.Fatal;
            throw;
        }
    }

    public async Task ExecuteAsync()
    {
        try
        {
            await FetchDataToDeleteAsync();
            await DeleteNodesAsync();
            state = DeleteRootTaskState.Done;
            Progress = 100;
        }
        catch (Exception e)
        {
            logger.Error(e, "Unhandled exception happened in task {task} in state {state}", nameof(DeleteRootTask), state);
            state = DeleteRootTaskState.Fatal;
            throw;
        }
    }

    private async Task FetchDataToDeleteAsync()
    {
        state = DeleteRootTaskState.FetchingData;
        await ProcessNodeAsync(rootId, CountDirectoriesToDeleteAsync);
    }

    private async Task CountDirectoriesToDeleteAsync(FileSystemNode[] nodes)
    {
        var directoryIds = nodes.Where(x => x.Type == NodeType.Directory).Select(x => x.Id).ToArray();
        totalDirectories += directoryIds.Length;
        foreach (var directoryId in directoryIds)
        {
            await ProcessNodeAsync(directoryId, CountDirectoriesToDeleteAsync);
        }
    }

    private async Task DeleteNodesAsync()
    {
        state = DeleteRootTaskState.Deleting;
        await rootsRepository.DeleteAsync(rootId);
        await ProcessNodeAsync(rootId, DeleteNodesAsync);
    }

    private async Task DeleteNodesAsync(FileSystemNode[] nodes)
    {
        var directoryIds = nodes.Where(x => x.Type == NodeType.Directory).Select(x => x.Id).ToArray();
        var fileIds = nodes.Where(x => x.Type == NodeType.File).Select(x => x.Id).ToArray();

        await nodesRepository.DeleteManyAsync(nodes.Select(x => x.Id).ToArray());
        await tagsRepository.DeleteManyAsync(fileIds);
        UpdateProgress(() => processedDirectories++);

        foreach (var directoryId in directoryIds)
        {
            await ProcessNodeAsync(directoryId, DeleteNodesAsync);
        }
    }

    private async Task ProcessNodeAsync(Guid nodeId, Func<FileSystemNode[], Task> func)
    {
        var nodes = await nodesRepository.ReadAllFilesAsync(nodeId, false, includeHidden: true);
        await func(nodes);
    }

    private void UpdateProgress(Action action)
    {
        action();
        Progress = processedDirectories * 100 / totalDirectories;
    }

    public Guid Id { get; }
    public string CurrentState => state.ToString();
    public string TaskType => BackgroundTaskType.DeleteRoot.ToString();
    public int Progress { get; private set; }


    private int processedDirectories;
    private int totalDirectories;
    private Guid rootId;
    private DeleteRootTaskState state;
    private readonly IRootsRepository rootsRepository;
    private readonly INodesRepository nodesRepository;
    private readonly ITagsRepository tagsRepository;
    private readonly ILogger logger;
}