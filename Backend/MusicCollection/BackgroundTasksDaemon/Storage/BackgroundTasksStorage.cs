using System.Collections.Concurrent;
using BackgroundTasksDaemon.Builder;
using BackgroundTasksDaemon.Tasks;
using MusicCollection.Api.Dto.Admin;

namespace BackgroundTasksDaemon.Storage;

public class BackgroundTasksStorage : IBackgroundTasksStorage
{
    public BackgroundTasksStorage(IBackgroundTaskBuilder backgroundTaskBuilder)
    {
        this.backgroundTaskBuilder = backgroundTaskBuilder;
        queue = new ConcurrentQueue<IBackgroundTask>();
        tasks = new ConcurrentDictionary<Guid, IBackgroundTask>();
        tasksToRemove = new ConcurrentBag<Guid>();
    }

    public async Task<Guid> AddTask(BackgroundTaskType type, string[]? args)
    {
        var task = await backgroundTaskBuilder
            .ForTaskType(type)
            .WithArgs(args)
            .BuildAsync();

        tasks[task.Id] = task;
        queue.Enqueue(task);

        return task.Id;
    }

    public void RemoveTask(Guid id)
    {
        if (tasks.ContainsKey(id))
        {
            tasks.Remove(id, out _);
        }

        var taskInQueue = queue.FirstOrDefault(x => x.Id == id);
        if (taskInQueue != null)
        {
            tasksToRemove.Add(taskInQueue.Id);
        }
    }

    public IBackgroundTask? TryGetNextTask()
    {
        var task = queue.TryDequeue(out var t) ? t : null;
        if (task == null)
        {
            return null;
        }

        var removedTasks = tasksToRemove.Where(x => x == task.Id).ToArray();
        return removedTasks.Any() ? TryGetNextTask() : task;
    }

    public TaskStateDto[] GetTasks(Guid id)
    {
        return tasks.Values.Select(x => new TaskStateDto
        {
            Progress = x.Progress,
            State = x.CurrentState
        }).ToArray();
    }

    public TaskStateDto GetTaskState(Guid id)
    {
        var task = tasks.TryGetValue(id, out var t)
            ? t
            : throw new InvalidOperationException($"Task {id} was not found");

        return new TaskStateDto
        {
            Progress = task.Progress,
            State = task.CurrentState
        };
    }

    private readonly IBackgroundTaskBuilder backgroundTaskBuilder;
    private readonly ConcurrentDictionary<Guid, IBackgroundTask> tasks;
    private readonly ConcurrentQueue<IBackgroundTask> queue;
    private readonly ConcurrentBag<Guid> tasksToRemove;
}