namespace BackgroundTasksDaemon.Tasks.CreateRoot;

public enum CreateRootTaskState
{
    Pending,
    FetchingDirectories,
    CreatingNodes,
    CreatingTags
}