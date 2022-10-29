using MusicCollection.BusinessLogic.Repositories.Files.Nodes;
using MusicCollection.BusinessLogic.Repositories.Files.Roots;
using MusicCollection.Common.TagsService;

namespace BackgroundTasksDaemon.Tasks.CreateRoot;

public class CreateRootTask : IBackgroundTask
{
    public CreateRootTask(
        IRootsRepository rootsRepository,
        INodesRepository nodesRepository,
        ITagsExtractor tagsExtractor
    )
    {
        Id = Guid.NewGuid();
        state = CreateRootTaskState.Pending;
    }

    public Task ExecuteAsync()
    {
        throw new NotImplementedException();
    }

    private void FetchDirectories()
    {
        state = CreateRootTaskState.FetchingDirectories;
    }

    private async Task CreateNodesAsync()
    {
        state = CreateRootTaskState.CreatingNodes;
    }

    private async Task CreateTagsAsync()
    {
        state = CreateRootTaskState.CreatingTags;
    }

    public Guid Id { get; set; }
    public string CurrentState => state.ToString();
    public int Progress => progress;

    private CreateRootTaskState state;
    private int progress;
}