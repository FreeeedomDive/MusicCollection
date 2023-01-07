using BackgroundTasksDaemon.Storage;
using TelemetryApp.Api.Client.Log;

namespace BackgroundTasksDaemon;

public class TasksDaemon : ITasksDaemon
{
    public TasksDaemon(
        IBackgroundTasksStorage backgroundTasksStorage,
        ILoggerClient logger
    )
    {
        this.backgroundTasksStorage = backgroundTasksStorage;
        this.logger = logger;
    }

    public async Task Start()
    {
        await logger.InfoAsync("{Daemon} has started", nameof(TasksDaemon));
        while (true)
        {
            const int secondsToSleep = 10;
            await Task.Delay(secondsToSleep * 1000);
            var nextTask = backgroundTasksStorage.TryGetNextTask();

            if (nextTask == null)
            {
                continue;
            }

            await logger.InfoAsync("Task {Id} is starting", nextTask.Id);
            try
            {
                await nextTask.ExecuteAsync();
            }
            catch
            {
                await logger.InfoAsync("Task {Id} has failed", nextTask.Id);
            }
        }
    }

    private readonly IBackgroundTasksStorage backgroundTasksStorage;
    private readonly ILoggerClient logger;
}