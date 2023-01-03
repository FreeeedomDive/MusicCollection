using Microsoft.AspNetCore.Mvc;
using MusicCollection.Api.Dto.Exceptions;
using MusicCollection.Api.Dto.Users;
using MusicCollection.BusinessLogic.Services.UsersService;


namespace MusicCollection.Controllers;

[ApiController]
[Route("users")]
public class UsersController : Controller
{
    private readonly IUsersService usersService;

    public UsersController(IUsersService usersService)
    {
        this.usersService = usersService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> Login([FromBody] AuthCredentials authCredentials)
    {
        try
        {
            return await usersService.LoginAsync(authCredentials);
        }
        catch (UserNotFoundException exception)
        {
            return NotFound();
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register([FromBody] AuthCredentials authCredentials)
    {
        try
        {
            return await usersService.RegisterAsync(authCredentials);
        }
        catch (UserWithLoginAlreadyExistsException)
        {
            return Conflict();
        }
    }
}