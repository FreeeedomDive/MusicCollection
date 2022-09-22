using Microsoft.AspNetCore.Mvc;
using MusicCollection.Api.Dto.Auth;
using MusicCollection.Api.Dto.Exceptions;
using MusicCollection.BusinessLogic.Services.AuthService;


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
    public async Task<ActionResult<User>> FindUser([FromBody]AuthCredentials authCredentials)
    {
        try
        {
            return await usersService.ReadAsync(authCredentials);
        }
        catch (UserNotFoundException e)
        {
            return NotFound();
        }
    }
    
    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser([FromBody]AuthCredentials authCredentials)
    {
        try
        {
            await usersService.CreateAsync(authCredentials);
            return Ok();
        }
        catch (UserNotFoundException e)
        {
            return NotFound();
        }
    }
}