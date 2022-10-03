using Microsoft.AspNetCore.Mvc;
using MusicCollection.Api.Dto.Admin;
using MusicCollection.BusinessLogic.Services.FilesService;
using DirectoryNotFoundException = MusicCollection.Api.Dto.Exceptions.DirectoryNotFoundException;

namespace MusicCollection.Controllers;

[ApiController]
[Route("admin")]
public class AdminController : Controller
{
    public AdminController(
        IFilesService filesService
    )
    {
        this.filesService = filesService;
    }

    [HttpPost("roots/create")]
    public async Task<ActionResult<Guid>> CreateNode([FromBody] CreateRootRequest request)
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