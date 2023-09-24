namespace BackgroundTasksDaemon.Tasks;

public interface IBackgroundTask
{
    Task InitializeWithArgsAsync(string[]? args = null);
    Task ExecuteAsync();
    Guid Id { get; }
    string CurrentState { get; }
    string TaskType { get; }
    int Progress { get; }
}