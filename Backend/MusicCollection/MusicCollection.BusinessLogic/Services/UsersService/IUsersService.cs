using MusicCollection.Api.Dto.Auth;
using MusicCollection.BusinessLogic.Repositories.Auth;

namespace MusicCollection.BusinessLogic.Services.AuthService;

public interface IUsersService
{
    Task<User> ReadAsync(Guid id);
    Task<User?> TryReadAsync(Guid id);
    Task<Guid> CreateOrUpdateAsync(User user);
    Task<Guid> CreateAsync(User user);
    Task<Guid> UpdateAsync(User user);
    Task<bool> TryDeleteAsync(User user);
}