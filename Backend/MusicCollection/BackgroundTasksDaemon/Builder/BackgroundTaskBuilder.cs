using BackgroundTasksDaemon.Tasks;
using BackgroundTasksDaemon.Tasks.CreateRoot;
using BackgroundTasksDaemon.Tasks.DeleteRoot;
using TelemetryApp.Api.Client.Log;

namespace BackgroundTasksDaemon.Builder;

public class BackgroundTaskBuilder : IBackgroundTaskBuilder
{
    public BackgroundTaskBuilder(
        ILoggerClient logger,
        IRootsRepository rootsRepository,
        INodesRepository nodesRepository,
        ITagsRepository tagsRepository,
        ITagsExtractor tagsExtractor
    )
    {
        this.logger = logger;
        this.rootsRepository = rootsRepository;
        this.nodesRepository = nodesRepository;
        this.tagsRepository = tagsRepository;
        this.tagsExtractor = tagsExtractor;
    }

    public IBackgroundTaskBuilder ForTaskType(BackgroundTaskType type)
    {
        Type = type;

        return this;
    }

    public IBackgroundTaskBuilder WithArgs(string[]? args)
    {
        Args = args;

        return this;
    }

    public async Task<IBackgroundTask> BuildAsync()
    {
        IBackgroundTask task = Type switch
        {
            BackgroundTaskType.CreateRoot => 
                new CreateRootTask(rootsRepository, nodesRepository, tagsExtractor, tagsRepository, logger),
            BackgroundTaskType.DeleteRoot => 
                new DeleteRootTask(rootsRepository, nodesRepository, tagsRepository, logger),
            _ => throw new ArgumentOutOfRangeException()
        };

        await task.InitializeWithArgsAsync(Args);
        return task;
    }

    private BackgroundTaskType Type { get; set; }
    private string[]? Args { get; set; }

    private readonly ILoggerClient logger;
    private readonly IRootsRepository rootsRepository;
    private readonly INodesRepository nodesRepository;
    private readonly ITagsRepository tagsRepository;
    private readonly ITagsExtractor tagsExtractor;
}