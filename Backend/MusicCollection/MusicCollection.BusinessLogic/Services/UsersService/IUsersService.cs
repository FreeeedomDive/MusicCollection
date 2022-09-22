using MusicCollection.Api.Dto.Auth;
using MusicCollection.BusinessLogic.Repositories.Auth;

namespace MusicCollection.BusinessLogic.Services.AuthService;

public interface IUsersService
{
    Task<User> ReadAsync(Guid id);
    Task<User> ReadAsync(AuthCredentials authCredentials);
    Task<User?> TryReadAsync(Guid id);
    Task CreateAsync(AuthCredentials authCredentials);
}