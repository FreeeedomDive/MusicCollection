using MusicCollection.Api.Dto.Auth;

namespace MusicCollection.BusinessLogic.Repositories.Auth;

public interface IUsersRepository
{
    Task<User> ReadAsync(Guid id);
    Task<User?> TryReadAsync(Guid id);
    Task<Guid> CreateOrUpdateAsync(User user);
    Task<Guid> CreateAsync(User user);
    Task<Guid> UpdateAsync(User user);
    Task<bool> TryDeleteAsync(User user);
}