using BackgroundTasksDaemon.Tasks;

namespace BackgroundTasksDaemon.Builder;

public interface IBackgroundTaskBuilder
{
    IBackgroundTaskBuilder ForTaskType(BackgroundTaskType type);
    IBackgroundTaskBuilder WithArgs(string[]? args);
    Task<IBackgroundTask> BuildAsync();
}