namespace BackgroundTasksDaemon;

public interface IBackgroundTask
{
    Guid Id { get; set; }
    string CurrentState { get; set; }
    int Progress { get; set; }

    Task ExecuteAsync();
}