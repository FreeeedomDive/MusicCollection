using Microsoft.AspNetCore.Mvc;
using MusicCollection.Api.Dto.Auth;
using MusicCollection.Api.Dto.Exceptions;
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

    [HttpPost("find")]
    public async Task<ActionResult<User>> FindUser([FromBody] AuthCredentials authCredentials)
    {
        try
        {
            return await usersService.FindAsync(authCredentials);
        }
        catch (UserNotFoundException exception)
        {
            return NotFound();
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> RegisterUser([FromBody] AuthCredentials authCredentials)
    {
        return await usersService.CreateAsync(authCredentials);
    }
}