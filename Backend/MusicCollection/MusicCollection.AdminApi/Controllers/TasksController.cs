using BackgroundTasksDaemon.Storage;
using Microsoft.AspNetCore.Mvc;
using MusicCollection.Api.Dto.Admin;

namespace MusicCollection.AdminApi.Controllers;

[ApiController]
[Route("adminApi/[controller]")]
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

    [HttpDelete("{id:guid}")]
    public void Delete([FromRoute] Guid id)
    {
        backgroundTasksStorage.RemoveTask(id);
    }
    
    private readonly IBackgroundTasksStorage backgroundTasksStorage;
}