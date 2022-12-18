namespace BackgroundTasksDaemon.Tasks.CreateRoot;

public enum CreateRootTaskState
{
    Pending,
    Fatal,
    FetchingDirectories,
    CreatingNodes,
    CreatingTags,
    Done
}