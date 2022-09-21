using Microsoft.AspNetCore.Mvc;

namespace MusicCollection.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}