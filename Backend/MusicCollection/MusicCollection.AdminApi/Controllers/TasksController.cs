using BackgroundTasksDaemon.Storage;
using Microsoft.AspNetCore.Mvc;
using MusicCollection.Api.Dto.Admin;

namespace MusicCollection.AdminApi.Controllers;

[ApiController]
[Route("tasks")]
public class TasksController : Controller
{
    public TasksController(IBackgroundTasksStorage backgroundTasksStorage)
    {
        this.backgroundTasksStorage = backgroundTasksStorage;
    }
    
    [HttpGet]
    public TaskDto[] GetAll()
    {
        return backgroundTasksStorage.GetTasks();
    }

    [HttpDelete]
    public void RemoveFromQueue(Guid id)
    {
        backgroundTasksStorage.RemoveTask(id);
    }
    
    private readonly IBackgroundTasksStorage backgroundTasksStorage;
}