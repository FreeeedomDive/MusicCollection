using Microsoft.AspNetCore.Mvc;
using MusicCollection.Api.Dto.Admin;
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
    
    [HttpPost("roots/create")]
    public async Task<ActionResult<Guid>> CreateRoot([FromBody] CreateRootRequest request)
    {
        try
        {
            return await filesService.CreateRootWithIndexAsync(request.Path);
        }
        catch(DirectoryNotFoundException)
        {
            return NotFound();
        }
    }

    private readonly IFilesService filesService;
}