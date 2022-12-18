using BackgroundTasksDaemon.Storage;

namespace BackgroundTasksDaemon;

public class TasksDaemon : ITasksDaemon
{
    public TasksDaemon(IBackgroundTasksStorage backgroundTasksStorage)
    {
        this.backgroundTasksStorage = backgroundTasksStorage;
    }

    public async Task Start()
    {
        while (true)
        {
            const int secondsToSleep = 30;
            await Task.Delay(secondsToSleep * 1000);
            var nextTask = backgroundTasksStorage.TryGetNextTask();

            if (nextTask == null)
            {
                return;
            }

            await nextTask.ExecuteAsync();
        }
    }

    private readonly IBackgroundTasksStorage backgroundTasksStorage;
}