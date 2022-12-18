using BackgroundTasksDaemon.Tasks;
using MusicCollection.Api.Dto.Admin;

namespace BackgroundTasksDaemon.Storage;

public interface IBackgroundTasksStorage
{
    Task<Guid> AddTask(BackgroundTaskType type, string[]? args);
    void RemoveTask(Guid id);
    IBackgroundTask? TryGetNextTask();
    TaskDto[] GetTasks();
}