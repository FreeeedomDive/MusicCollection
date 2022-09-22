using Microsoft.AspNetCore.Mvc;
using MusicCollection.Api.Dto.Exceptions;
using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.BusinessLogic.Repositories.Files;
using MusicCollection.BusinessLogic.Services.FilesService;

namespace MusicCollection.Controllers;

[ApiController]
[Route("files")]
public class FilesController : Controller
{
    private readonly IFilesService filesService;

    public FilesController(IFilesService filesService)
    {
        this.filesService = filesService;
    }

    [HttpGet("nodes/{id:guid}")]
    public async Task<ActionResult<FileSystemNode>> GetNode([FromRoute] Guid id)
    {
        try
        {
            return Ok(await filesService.ReadNodeAsync(id));
        }
        catch (FileSystemNodeNotFoundException e)
        {
            return NotFound();
        }
    }
    
    [HttpGet("roots/{id:guid}")]
    public async Task<ActionResult<FileSystemNode>> GetRoot([FromRoute] Guid id)
    {
        try
        {
            return Ok(await filesService.ReadRootAsync(id));
        }
        catch (FileSystemNodeNotFoundException e)
        {
            return NotFound();
        }
    }

    [HttpGet("nodes/{parentId:guid}/files")]
    public async Task<ActionResult<List<FileSystemNode>>> GetAllFiles([FromRoute] Guid parentId)
    {
        return await filesService.ReadAllFiles(parentId);
    }

    [HttpPost("nodes/{id:guid}")]
    public async Task<ActionResult> UpdateNode([FromBody] FileSystemNode node)
    {
        await filesService.UpdateNodeAsync(node);
        return Ok();
    }

    [HttpDelete("nodes/{id:guid}")]
    public async Task<ActionResult<bool>> DeleteNode([FromBody] FileSystemNode node)
    {
        await filesService.TryDeleteNodeAsync(node);
        return Ok();
    }
    
    [HttpDelete("roots/{id:guid}")]
    public async Task<ActionResult<bool>> DeleteRoot([FromBody] FileSystemRoot root)
    {
        await filesService.TryDeleteRootAsync(root);
        return Ok();
    }
}