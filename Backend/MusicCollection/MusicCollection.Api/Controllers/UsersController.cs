using Microsoft.AspNetCore.Mvc;
using MusicCollection.Api.Dto.Users;
using MusicCollection.BusinessLogic.Services.UsersService;

namespace MusicCollection.Controllers;

[ApiController, Route("api/[controller]")]
public class UsersController : Controller
{
    public UsersController(IUsersService usersService)
    {
        this.usersService = usersService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> Login([FromBody] AuthCredentials authCredentials)
    {
        return await usersService.LoginAsync(authCredentials);
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register([FromBody] AuthCredentials authCredentials)
    {
        return await usersService.RegisterAsync(authCredentials);
    }

    private readonly IUsersService usersService;
}