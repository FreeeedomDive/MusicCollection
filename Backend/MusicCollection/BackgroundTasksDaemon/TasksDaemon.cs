using BackgroundTasksDaemon.Storage;
using MusicCollection.Common.Loggers;

namespace BackgroundTasksDaemon;

public class TasksDaemon : ITasksDaemon
{
    public TasksDaemon(
        IBackgroundTasksStorage backgroundTasksStorage,
        ILogger logger
    )
    {
        this.backgroundTasksStorage = backgroundTasksStorage;
        this.logger = logger;
    }

    public async Task Start()
    {
        logger.Info("{Daemon} has started", nameof(TasksDaemon));
        while (true)
        {
            const int secondsToSleep = 10;
            await Task.Delay(secondsToSleep * 1000);
            var nextTask = backgroundTasksStorage.TryGetNextTask();

            if (nextTask == null)
            {
                continue;
            }

            logger.Info("Task {Id} is starting", nextTask.Id);
            try
            {
                await nextTask.ExecuteAsync();
            }
            catch
            {
                logger.Info("Task {Id} has failed", nextTask.Id);
            }
        }
    }

    private readonly IBackgroundTasksStorage backgroundTasksStorage;
    private readonly ILogger logger;
}