using Microsoft.AspNetCore.Mvc;
using MusicCollection.Api.Dto.Exceptions;
using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.MusicService.Services.FilesService;

namespace MusicCollection.Controllers;

[ApiController]
[Route("api/[controller]")]
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

    [HttpGet("roots/{rootId:guid}")]
    public async Task<ActionResult<FileSystemRoot>> GetRoot([FromRoute] Guid rootId)
    {
        return await filesService.ReadRootAsync(rootId);
    }

    [HttpGet("nodes/{nodeId:guid}")]
    public async Task<ActionResult<FileSystemNode>> GetNode([FromRoute] Guid nodeId)
    {
        return  await filesService.ReadNodeAsync(nodeId);
    }

    [HttpGet("nodes/{nodeId:guid}/ReadChildren")]
    public async Task<ActionResult<FileSystemNode[]>> ReadDirectory([FromRoute] Guid nodeId, [FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        return await filesService.ReadDirectoryAsync(nodeId, skip, take);
    }

    [HttpGet("nodes/{nodeId:guid}/ReadAll")]
    public async Task<ActionResult<Guid[]>> ReadAllFiles([FromRoute] Guid nodeId)
    {
        return await filesService.ReadAllFilesFromDirectoryAsync(nodeId);
    }

    [HttpGet("nodes/{nodeId:guid}/Download")]
    public async Task DownloadFileContent([FromRoute] Guid nodeId)
    {
        var result = await filesService.ReadFileContentAsync(nodeId);
        await Response.Body.WriteAsync(result);
    }

    [HttpGet("nodes/{nodeId:guid}/DownloadStream")]
    public async Task<IActionResult> DownloadFileContentAsStream([FromRoute] Guid nodeId)
    {
        var (stream, mimeType) = await filesService.ReadFileContentAsStreamAsync(nodeId);
        var result = File(stream, mimeType);
        result.EnableRangeProcessing = true;
        return result;
    }

    private readonly IFilesService filesService;
}