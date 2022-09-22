using MusicCollection.Api.Dto.Auth;

namespace MusicCollection.BusinessLogic.Repositories.Auth;

public interface IUsersRepository
{
    Task<User> ReadAsync(Guid id);
    Task<User> ReadAsync(string login, string encryptedPassword);
    Task<User?> TryReadAsync(Guid id);
    Task CreateAsync(string login, string encryptedPassword);
}