namespace BackgroundTasksDaemon.Tasks;

public interface IBackgroundTask
{
    Guid Id { get; }
    string CurrentState { get; }
    int Progress { get; }

    Task ExecuteAsync();
}