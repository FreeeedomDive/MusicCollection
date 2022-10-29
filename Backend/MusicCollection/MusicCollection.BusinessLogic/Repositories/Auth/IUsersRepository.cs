using MusicCollection.Api.Dto.Auth;

namespace MusicCollection.BusinessLogic.Repositories.Auth;

public interface IUsersRepository : IMusicCollectionRepository
{
    Task<User[]> ReadAllAsync();
    Task<User?> FindAsync(string login);
    Task<User?> FindAsync(string login, string encryptedPassword);
    Task<User?> TryReadAsync(Guid id);
    Task<User> CreateAsync(string login, string encryptedPassword);
    Task DeleteAsync(Guid id);
}