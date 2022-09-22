using Microsoft.AspNetCore.Mvc;
using MusicCollection.Api.Dto.Exceptions;
using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.BusinessLogic.Repositories.Files;
using MusicCollection.BusinessLogic.Services.FilesService;

namespace MusicCollection.Controllers;

[ApiController]
[Route("roots")]
public class FilesController : Controller
{
    private readonly IFilesService filesService;

    public FilesController(IFilesService filesService)
    {
        this.filesService = filesService;
    }

    [HttpGet("{rootId:guid}/nodes/{id:guid}")]
    public async Task<ActionResult<FileSystemNode>> GetNode([FromRoute] Guid id)
    {
        try
        {
            return await filesService.ReadNodeAsync(id);
        }
        catch (FileSystemNodeNotFoundException e)
        {
            return NotFound();
        }
    }

    [HttpGet("{rootId:guid}/{parentId:guid}")]
    public async Task<ActionResult<FileSystemNode[]>> GetAllFiles([FromRoute] Guid parentId)
    {
        return await filesService.ReadAllFiles(parentId);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetAllRoots()
    {
        return Ok(await filesService.ReadAllRoots());
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FileSystemRoot>> GetRoot([FromRoute] Guid id)
    {
        try
        {
            return await filesService.ReadRootAsync(id);
        }
        catch (RootNotFoundException e)
        {
            return NotFound();
        }
    }
}