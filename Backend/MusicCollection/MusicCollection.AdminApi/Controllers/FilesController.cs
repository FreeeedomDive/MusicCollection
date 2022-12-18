using BackgroundTasksDaemon;
using BackgroundTasksDaemon.Storage;
using BackgroundTasksDaemon.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicCollection.Api.Dto.Admin;
using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.BusinessLogic.Services.FilesService;
using DirectoryNotFoundException = MusicCollection.Api.Dto.Exceptions.DirectoryNotFoundException;

namespace MusicCollection.AdminApi.Controllers;

[ApiController]
[Route("files")]
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

    [HttpPost("nodes/{nodeId:guid}/hide")]
    public async Task<ActionResult> HideNode(Guid nodeId)
    {
        try
        {
            await filesService.HideNodeAsync(nodeId);
            return Ok();
        }
        catch (DirectoryNotFoundException)
        {
            return NotFound();
        }
    }

    private readonly IFilesService filesService;
    private readonly IBackgroundTasksStorage backgroundTasksStorage;
}