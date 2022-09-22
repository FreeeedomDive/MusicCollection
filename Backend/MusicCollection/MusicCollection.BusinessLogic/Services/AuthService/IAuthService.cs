using MusicCollection.Api.Dto.Auth;
using MusicCollection.BusinessLogic.Repositories.Auth;

namespace MusicCollection.BusinessLogic.Services.AuthService;

public interface IAuthService
{
    Task<User> ReadAsync(Guid id);
    Task<User?> TryReadAsync(Guid id);
    Task CreateOrUpdateAsync(User user);
    Task CreateAsync(User user);
    Task UpdateAsync(User user);
    Task<bool> TryDeleteAsync(User user);
}