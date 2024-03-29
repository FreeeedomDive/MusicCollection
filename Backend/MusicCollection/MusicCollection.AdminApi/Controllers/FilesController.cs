using BackgroundTasksDaemon.Storage;
using BackgroundTasksDaemon.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicCollection.Api.Dto.Admin;
using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.BusinessLogic.Services.FilesService;

namespace MusicCollection.AdminApi.Controllers;

[ApiController, Route("adminApi/[controller]")]
public class FilesController : Controller
{
    public FilesController(
        IFilesService filesService,
        IBackgroundTasksStorage backgroundTasksStorage
    )
    {
        this.filesService = filesService;
        this.backgroundTasksStorage = backgroundTasksStorage;
    }

    [HttpGet("roots")]
    public async Task<ActionResult<FileSystemRoot[]>> GetAllRoots()
    {
        return await filesService.ReadAllRoots();
    }

    [HttpPost("roots/create")]
    public async Task<ActionResult<Guid>> CreateRoot([FromBody] CreateRootRequest request)
    {
        return await backgroundTasksStorage.AddTask(BackgroundTaskType.CreateRoot, new[] { request.Name, request.Path });
    }

    [HttpDelete("roots/{rootId:guid}")]
    public async Task<ActionResult<Guid>> DeleteRoot([FromRoute] Guid rootId)
    {
        return await backgroundTasksStorage.AddTask(BackgroundTaskType.DeleteRoot, new[] { rootId.ToString() });
    }

    [HttpPost("nodes/{nodeId:guid}/hide")]
    public async Task<ActionResult> HideNode(Guid nodeId)
    {
        await filesService.HideNodeAsync(nodeId);
        return Ok();
    }

    private readonly IBackgroundTasksStorage backgroundTasksStorage;

    private readonly IFilesService filesService;
}