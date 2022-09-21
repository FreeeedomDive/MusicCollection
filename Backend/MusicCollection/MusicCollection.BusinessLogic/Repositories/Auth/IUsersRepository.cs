using MusicCollection.Api.Dto.Auth;

namespace MusicCollection.BusinessLogic.Repositories.Auth;

public interface IUsersRepository
{
    Task<User> ReadAsync(Guid id);
    Task<User?> TryReadAsync(Guid id);
    Task CreateOrUpdateAsync(User user);
    Task CreateAsync(User user);
    Task UpdateAsync(User user);
    Task<bool> TryDeleteAsync(User user);
}