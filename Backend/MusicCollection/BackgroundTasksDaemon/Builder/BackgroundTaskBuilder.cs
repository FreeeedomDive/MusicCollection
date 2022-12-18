using BackgroundTasksDaemon.Tasks;
using BackgroundTasksDaemon.Tasks.CreateRoot;
using MusicCollection.BusinessLogic.Repositories.Files.Nodes;
using MusicCollection.BusinessLogic.Repositories.Files.Roots;
using MusicCollection.BusinessLogic.Repositories.Files.Tags;
using MusicCollection.Common.Loggers;
using MusicCollection.Common.TagsService;

namespace BackgroundTasksDaemon.Builder;

public class BackgroundTaskBuilder : IBackgroundTaskBuilder
{
    public BackgroundTaskBuilder(
        ILogger logger,
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
        switch (Type)
        {
            case BackgroundTaskType.CreateRoot:
                var task = new CreateRootTask(rootsRepository, nodesRepository, tagsExtractor, tagsRepository, logger);
                await task.InitializeAsync(Args);
                return task;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private BackgroundTaskType Type { get; set; }
    private string[]? Args { get; set; }

    private readonly ILogger logger;
    private readonly IRootsRepository rootsRepository;
    private readonly INodesRepository nodesRepository;
    private readonly ITagsRepository tagsRepository;
    private readonly ITagsExtractor tagsExtractor;
}