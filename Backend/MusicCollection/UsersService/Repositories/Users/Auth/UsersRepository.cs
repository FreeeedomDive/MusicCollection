using MusicCollection.Api.Dto.Users;
using MusicCollection.BusinessLogic.Repositories.Auth;
using SqlRepositoryBase.Core.Repository;

namespace MusicCollection.UsersService.Repositories.Users.Auth;

public class UsersRepository : IUsersRepository
{
    public UsersRepository(ISqlRepository<UserStorageElement> sqlRepository)
    {
        this.sqlRepository = sqlRepository;
    }

    public async Task<User[]> ReadAllAsync()
    {
        var results = await sqlRepository.ReadAllAsync();
        return results.Select(ToModel).ToArray()!;
    }

    public async Task<User?> FindAsync(string login)
    {
        var results = await sqlRepository.FindAsync(user => user.Login == login);
        return ToModel(results.FirstOrDefault());
    }

    public async Task<User?> FindAsync(string login, string encryptedPassword)
    {
        var results = await sqlRepository.FindAsync(user => user.Login == login && user.Password == encryptedPassword);
        return ToModel(results.FirstOrDefault());
    }

    public async Task<User?> TryReadAsync(Guid id)
    {
        var result = await sqlRepository.TryReadAsync(id);
        return ToModel(result);
    }

    public async Task<User> CreateAsync(string login, string encryptedPassword)
    {
        var newUser = new UserStorageElement
        {
            Id = Guid.NewGuid(),
            Login = login,
            Password = encryptedPassword
        };
        await sqlRepository.CreateAsync(newUser);
        return ToModel(newUser)!;
    }

    public async Task DeleteAsync(Guid id)
    {
        await sqlRepository.DeleteAsync(id);
    }

    private static User? ToModel(UserStorageElement? user)
    {
        return user == null
            ? null
            : new User
            {
                Id = user.Id,
                Login = user.Login,
            };
    }
    
    private readonly ISqlRepository<UserStorageElement> sqlRepository;
}