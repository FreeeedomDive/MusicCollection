using Microsoft.AspNetCore.Mvc;
using MusicCollection.Api.Dto.Auth;
using MusicCollection.BusinessLogic.Repositories.Auth;

namespace MusicCollection.AdminApi.Controllers;

[ApiController]
[Route("users")]
public class UsersController : Controller
{
    public UsersController(IUsersRepository usersRepository)
    {
        this.usersRepository = usersRepository;
    }

    [HttpGet]
    public async Task<User[]> GetAll()
    {
        return await usersRepository.ReadAllAsync();
    }
    
    [HttpDelete("{userId:guid}/ban")]
    public async Task Ban(Guid userId)
    {
        await usersRepository.DeleteAsync(userId);
    }

    private readonly IUsersRepository usersRepository;
}