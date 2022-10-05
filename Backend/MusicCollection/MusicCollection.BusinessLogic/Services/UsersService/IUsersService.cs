using MusicCollection.Api.Dto.Auth;

namespace MusicCollection.BusinessLogic.Services.UsersService;

public interface IUsersService
{
    Task<User> LoginAsync(AuthCredentials authCredentials);
    Task<User> RegisterAsync(AuthCredentials authCredentials);
}