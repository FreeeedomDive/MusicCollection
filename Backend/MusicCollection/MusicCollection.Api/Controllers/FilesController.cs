using Microsoft.AspNetCore.Mvc;

namespace MusicCollection.Controllers;

[ApiController]
public class FilesController : ControllerBase
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}