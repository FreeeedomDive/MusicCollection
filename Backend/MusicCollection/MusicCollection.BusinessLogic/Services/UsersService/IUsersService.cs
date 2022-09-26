using MusicCollection.Api.Dto.Auth;
using MusicCollection.BusinessLogic.Repositories.Auth;

namespace MusicCollection.BusinessLogic.Services.AuthService;

public interface IUsersService
{
    Task<User> FindAsync(AuthCredentials authCredentials);
    Task CreateAsync(AuthCredentials authCredentials);
}