using Microsoft.AspNetCore.Mvc;
using MusicCollection.Api.Dto.Exceptions;
using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.BusinessLogic.Services.FilesService;

namespace MusicCollection.Controllers;

[ApiController]
[Route("roots")]
public class FilesController : Controller
{
    public FilesController(IFilesService filesService)
    {
        this.filesService = filesService;
    }
    
    [HttpGet]
    public async Task<ActionResult<FileSystemRoot[]>> GetAllRoots()
    {
        return await filesService.ReadAllRoots();
    }
    
    [HttpGet("{rootId:guid}")]
    public async Task<ActionResult<FileSystemRoot>> GetRoot([FromRoute] Guid rootId)
    {
        try
        {
            return await filesService.ReadRootAsync(rootId);
        }
        catch (RootNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("{rootId:guid}/nodes/{nodeId:guid}")]
    public async Task<ActionResult<FileSystemNode>> GetNode([FromRoute] Guid rootId, [FromRoute] Guid nodeId)
    {
        try
        {
            return await filesService.ReadNodeAsync(nodeId);
        }
        catch (FileSystemNodeNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("{rootId:guid}/nodes/{nodeId:guid}/ReadChildren")]
    public async Task<ActionResult<FileSystemNode[]>> ReadDirectory([FromRoute] Guid rootId, [FromRoute] Guid nodeId)
    {
        try
        {
            return await filesService.ReadDirectoryAsync(nodeId);
        }
        catch (FileSystemNodeNotFoundException)
        {
            return NotFound();
        }
        catch (ReadFilesFromNonDirectoryException)
        {
            return Conflict();
        }
    }

    [HttpGet("{rootId:guid}/nodes/{nodeId:guid}/ReadAll")]
    public async Task<ActionResult<Guid[]>> ReadAllFiles([FromRoute] Guid rootId, [FromRoute] Guid nodeId)
    {
        try
        {
            return await filesService.ReadAllFilesFromDirectoryAsync(nodeId);
        }
        catch (FileSystemNodeNotFoundException)
        {
            return NotFound();
        }
        catch (ReadFilesFromNonDirectoryException)
        {
            return Conflict();
        }
    }

    private readonly IFilesService filesService;
}