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
    
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<User>> FindUser([FromRoute]Guid userId)
    {
        try
        {
            return await usersService.ReadAsync(userId);
        }
        catch (UserNotFoundException e)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult> CreateOrUpdateUser([FromBody]User user)
    {
        return Ok(await usersService.CreateOrUpdateAsync(user));
    }
}