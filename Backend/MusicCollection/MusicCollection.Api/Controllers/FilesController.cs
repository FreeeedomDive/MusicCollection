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
            var result = await filesService.ReadNodeAsync(nodeId);
            return result;
        }
        catch (FileSystemNodeNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("{rootId:guid}/nodes/{nodeId:guid}/ReadChildren")]
    public async Task<ActionResult<FileSystemNode[]>> ReadDirectory([FromRoute] Guid rootId, [FromRoute] Guid nodeId, [FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        try
        {
            return await filesService.ReadDirectoryAsync(nodeId, skip, take);
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

    [HttpGet("{rootId:guid}/nodes/{nodeId:guid}/Download")]
    public async Task<FileBytes> DownloadFileContent([FromRoute] Guid rootId, [FromRoute] Guid nodeId)
    {
        var result = await filesService.ReadFileContentAsync(nodeId);
        return new FileBytes
        {
            Content = result
        };
    }

    [HttpGet("{rootId:guid}/nodes/{nodeId:guid}/DownloadStream")]
    public async Task<IActionResult> DownloadFileContentAsStream([FromRoute] Guid rootId, [FromRoute] Guid nodeId)
    {
        var (stream, mimeType) = await filesService.ReadFileContentAsStreamAsync(nodeId);
        var result = File(stream, mimeType);
        result.EnableRangeProcessing = true;
        return result;
    }

    private readonly IFilesService filesService;
}