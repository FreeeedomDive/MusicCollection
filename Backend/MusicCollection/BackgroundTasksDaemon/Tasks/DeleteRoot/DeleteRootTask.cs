using MusicCollection.Api.Dto.Exceptions.Files;
using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.BusinessLogic.Repositories.Files.Nodes;
using MusicCollection.BusinessLogic.Repositories.Files.Roots;
using MusicCollection.BusinessLogic.Repositories.Files.Tags;
using TelemetryApp.Api.Client.Log;

namespace BackgroundTasksDaemon.Tasks.DeleteRoot;

public class DeleteRootTask : IBackgroundTask
{
    public DeleteRootTask(
        IRootsRepository rootsRepository,
        INodesRepository nodesRepository,
        ITagsRepository tagsRepository,
        ILoggerClient logger
    )
    {
        this.rootsRepository = rootsRepository;
        this.nodesRepository = nodesRepository;
        this.tagsRepository = tagsRepository;
        this.logger = logger;

        Id = Guid.NewGuid();
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
            await logger.ErrorAsync(e, "Unhandled exception happened in task {task} in state {state}", nameof(DeleteRootTask), state);
            state = DeleteRootTaskState.Fatal;
            throw;
        }
    }

    public Guid Id { get; }
    public string CurrentState => state.ToString();
    public string TaskType => BackgroundTaskType.DeleteRoot.ToString();
    public int Progress { get; private set; }

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
        await ProcessNodeAsync(rootId, DeleteNodesAsync);
        await nodesRepository.DeleteAsync(rootId);
        await rootsRepository.DeleteAsync(rootId);
    }

    private async Task DeleteNodesAsync(FileSystemNode[] nodes)
    {
        var directoryIds = nodes.Where(x => x.Type == NodeType.Directory).Select(x => x.Id).ToArray();
        var fileIds = nodes.Where(x => x.Type == NodeType.File).Select(x => x.Id).ToArray();

        // по дереву идем и удаляем все ноды снизу вверх
        foreach (var directoryId in directoryIds)
        {
            await ProcessNodeAsync(directoryId, DeleteNodesAsync);
        }

        await tagsRepository.DeleteManyAsync(fileIds);
        await nodesRepository.DeleteManyAsync(nodes.Select(x => x.Id).ToArray());
        UpdateProgress(() => processedDirectories++);
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

    private readonly ILoggerClient logger;
    private readonly INodesRepository nodesRepository;
    private readonly IRootsRepository rootsRepository;
    private readonly ITagsRepository tagsRepository;


    private int processedDirectories;
    private Guid rootId;
    private DeleteRootTaskState state;
    private int totalDirectories;
}