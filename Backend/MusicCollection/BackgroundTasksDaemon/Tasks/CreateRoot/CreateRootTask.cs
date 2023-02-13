using MusicCollection.Api.Dto.FileSystem;
using TelemetryApp.Api.Client.Log;

namespace BackgroundTasksDaemon.Tasks.CreateRoot;

public class CreateRootTask : IBackgroundTask
{
    public CreateRootTask(
        IRootsRepository rootsRepository,
        INodesRepository nodesRepository,
        ITagsExtractor tagsExtractor,
        ITagsRepository tagsRepository,
        ILoggerClient logger
    )
    {
        this.rootsRepository = rootsRepository;
        this.nodesRepository = nodesRepository;
        this.tagsExtractor = tagsExtractor;
        this.tagsRepository = tagsRepository;
        this.logger = logger;

        Id = Guid.NewGuid();
        state = CreateRootTaskState.Pending;
        allFileNodes = new List<FileSystemNode>();
        rootName = "";
        rootPath = "";
    }

    public Task InitializeWithArgsAsync(string[]? args = null)
    {
        if (args is null || args.Length != 2)
        {
            throw new InvalidOperationException(
                $"{nameof(CreateRootTask)} should have 2 args - root name and root path"
            );
        }

        rootName = args[0];
        rootPath = args[1];

        if (string.IsNullOrEmpty(rootName) || string.IsNullOrEmpty(rootPath))
        {
            throw new InvalidOperationException("Name and Path are empty");
        }

        if (Directory.Exists(rootPath))
        {
            return Task.CompletedTask;
        }

        state = CreateRootTaskState.Fatal;
        throw new InvalidOperationException($"Directory {rootPath} doesn't exist");
    }

    public async Task ExecuteAsync()
    {
        try
        {
            await FetchDirectories();
            await CreateNodesAsync();
            await CreateTagsAsync();
            state = CreateRootTaskState.Done;
            Progress = 100;
        }
        catch (Exception e)
        {
            await logger.ErrorAsync(e, "Unhandled exception happened in task {task} in state {state}", nameof(CreateRootTask), state);
            state = CreateRootTaskState.Fatal;
            throw;
        }
    }

    private async Task FetchDirectories()
    {
        state = CreateRootTaskState.FetchingDirectories;

        await ProcessDirectoryAsync(Guid.Empty, rootPath, CollectDirectoriesAndFilesAsync);

        await logger.InfoAsync($"Total {totalDirectories} directories and {totalFiles} files");
    }

    private async Task CollectDirectoriesAndFilesAsync(Guid _, string[] directories, string[] files)
    {
        totalDirectories += directories.Length;
        totalFiles += files.Length;

        foreach (var directory in directories)
        {
            await ProcessDirectoryAsync(Guid.Empty, directory, CollectDirectoriesAndFilesAsync);
        }
    }

    private async Task CreateNodesAsync()
    {
        state = CreateRootTaskState.CreatingNodes;

        var rootId = Guid.NewGuid();
        await rootsRepository.CreateAsync(new FileSystemRoot
        {
            Id = rootId,
            Name = rootName,
            Path = rootPath
        });
        await nodesRepository.CreateAsync(new FileSystemNode
        {
            Id = rootId,
            Path = rootPath,
            Type = NodeType.Directory
        });

        await ProcessDirectoryAsync(rootId, rootPath, CreateNodesAsync);
    }

    private async Task CreateNodesAsync(Guid parentId, string[] directories, string[] files)
    {
        var directoryNodes = BuildNodes(directories, parentId, NodeType.Directory);
        var fileNodes = BuildNodes(files, parentId, NodeType.File);

        var nodes = directoryNodes.Concat(fileNodes).ToArray();
        allFileNodes.AddRange(fileNodes);

        await nodesRepository.CreateManyAsync(nodes);

        UpdateProgress(() => processedFiles += fileNodes.Length);

        foreach (var directory in directoryNodes)
        {
            await ProcessDirectoryAsync(directory.Id, directory.Path, CreateNodesAsync);
            UpdateProgress(() => processedDirectories++);
        }
    }

    private static FileSystemNode[] BuildNodes(IEnumerable<string> paths, Guid parentId, NodeType type)
    {
        return paths.Select(x => new FileSystemNode
        {
            Id = Guid.NewGuid(),
            ParentId = parentId,
            Path = x,
            Type = type
        }).ToArray();
    }

    private void UpdateProgress(Action action)
    {
        action();
        Progress = (processedFiles * 100 / totalFiles + processedDirectories * 100 / totalDirectories) / 2;
    }

    private async Task CreateTagsAsync()
    {
        state = CreateRootTaskState.CreatingTags;
        Progress = 0;

        var current = 0;
        foreach (var node in allFileNodes)
        {
            var tags = tagsExtractor.TryExtractTags(node.Path);
            tags.Id = node.Id;
            await tagsRepository.CreateAsync(tags);

            current++;
            Progress = current * 100 / allFileNodes.Count;
        }
    }

    private static async Task ProcessDirectoryAsync(Guid nodeId, string path, Func<Guid, string[], string[], Task> func)
    {
        var directories = Directory.GetDirectories(path);
        var files = Directory.GetFiles(path).Where(x => x.IsSupportedExtension()).ToArray();

        if (directories.Length == 0 && files.Length == 0)
        {
            return;
        }

        await func(nodeId, directories, files);
    }

    public Guid Id { get; }
    public string CurrentState => state.ToString();
    public string TaskType => BackgroundTaskType.CreateRoot.ToString();
    public int Progress { get; private set; }

    private CreateRootTaskState state;

    private readonly IRootsRepository rootsRepository;
    private readonly INodesRepository nodesRepository;
    private readonly ITagsExtractor tagsExtractor;
    private readonly ITagsRepository tagsRepository;
    private readonly ILoggerClient logger;

    private string rootName;
    private string rootPath;

    private int processedDirectories;
    private int processedFiles;
    private int totalDirectories;
    private int totalFiles;

    private readonly List<FileSystemNode> allFileNodes;
}