using Microsoft.AspNetCore.Mvc;
using MusicCollection.Api.Dto.Users;
using MusicCollection.UserService.Services.UsersService;

namespace MusicCollection.UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
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
        return await usersService.LoginAsync(authCredentials);
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register([FromBody] AuthCredentials authCredentials)
    {
        return await usersService.RegisterAsync(authCredentials);
    }

    [HttpGet("settings/{userId:guid}")]
    public async Task<ActionResult<UserSettings>> GetOrCreateUserSettings([FromRoute] Guid userId)
    {
        return await usersService.ReadOrCreateSettingsAsync(userId);
    }
    
    [HttpPost("settings/{userId:guid}")]
    public async Task<ActionResult> UpdateSettings([FromRoute] Guid userId,
        [FromBody] UserSettings userSettings)
    {
        await usersService.UpdateSettingsAsync(userId, userSettings);
        return Ok();
    }
}