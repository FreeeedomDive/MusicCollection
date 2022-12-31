namespace BackgroundTasksDaemon.Tasks.DeleteRoot;

public enum DeleteRootTaskState
{
    Pending,
    Fatal,
    FetchingData,
    Deleting,
    Done
}