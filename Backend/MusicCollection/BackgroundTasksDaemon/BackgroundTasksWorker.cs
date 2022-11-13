using System.Collections.Concurrent;
using BackgroundTasksDaemon.Tasks;

namespace BackgroundTasksDaemon;

public class BackgroundTasksWorker
{
    public BackgroundTasksWorker()
    {
        queue = new ConcurrentQueue<IBackgroundTask>();
        tasks = new ConcurrentDictionary<Guid, IBackgroundTask>();
    }

    public Guid AddTask(IBackgroundTask task)
    {
        queue.Enqueue(task);
        tasks[task.Id] = task;
        return task.Id;
    }

    public string GetTaskState(Guid id)
    {
        return tasks.TryGetValue(id, out var task) ? task.CurrentState : $"Task {id} not found";
    }

    private readonly ConcurrentDictionary<Guid, IBackgroundTask> tasks;
    private readonly ConcurrentQueue<IBackgroundTask> queue;
}