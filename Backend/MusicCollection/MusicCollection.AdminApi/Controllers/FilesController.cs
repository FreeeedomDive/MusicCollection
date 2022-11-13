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
    public FilesController(IFilesService filesService)
    {
        this.filesService = filesService;
    }
    
    [HttpGet("roots")]
    public async Task<ActionResult<FileSystemRoot[]>> GetAllRoots()
    {
        return await filesService.ReadAllRoots();
    }
    
    [HttpPost("roots/create")]
    public async Task<ActionResult<Guid>> CreateRoot([FromBody] CreateRootRequest request)
    {
        try
        {
            return await filesService.CreateRootWithIndexAsync(request.Name, request.Path);
        }
        catch(DirectoryNotFoundException)
        {
            return NotFound();
        }
    }
    
    [HttpPost("nodes/{nodeId:guid}")]
    public async Task<ActionResult> HideNode(Guid nodeId)
    {
        try
        {
            await filesService.HideNodeAsync(nodeId);
            return Ok();
        }
        catch(DirectoryNotFoundException)
        {
            return NotFound();
        }
    }

    private readonly IFilesService filesService;
}