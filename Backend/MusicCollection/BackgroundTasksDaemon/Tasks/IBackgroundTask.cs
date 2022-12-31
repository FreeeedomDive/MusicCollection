namespace BackgroundTasksDaemon.Tasks;

public interface IBackgroundTask
{
    Guid Id { get; }
    string CurrentState { get; }
    string TaskType { get; }
    int Progress { get; }

    Task InitializeWithArgsAsync(string[]? args = null);
    Task ExecuteAsync();
}