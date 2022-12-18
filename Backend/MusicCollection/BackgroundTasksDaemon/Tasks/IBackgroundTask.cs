namespace BackgroundTasksDaemon.Tasks;

public interface IBackgroundTask
{
    Guid Id { get; }
    string CurrentState { get; }
    int Progress { get; }

    Task InitializeAsync(string[]? args = null);
    Task ExecuteAsync();
}