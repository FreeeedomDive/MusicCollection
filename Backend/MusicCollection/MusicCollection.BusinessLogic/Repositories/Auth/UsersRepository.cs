using DatabaseCore.Repository;
using MusicCollection.Api.Dto.Auth;
using MusicCollection.BusinessLogic.Repositories.Database;

namespace MusicCollection.BusinessLogic.Repositories.Auth;

public class UsersRepository : SqlRepository<UserStorageElement>, IUsersRepository
{
    public UsersRepository(DatabaseContext databaseContext): base(databaseContext, databaseContext.UsersStorage)
    {
    }

    public async Task<User?> FindAsync(string login)
    {
        var results = await FindAsync(user => user.Login == login);
        return ToModel(results.FirstOrDefault());
    }

    public async Task<User?> FindAsync(string login, string encryptedPassword)
    {
        var results = await FindAsync(user => user.Login == login && user.Password == encryptedPassword);
        return ToModel(results.FirstOrDefault());
    }

    public new async Task<User?> TryReadAsync(Guid id)
    {
        var result = await base.TryReadAsync(id);
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
        await CreateAsync(newUser);
        return ToModel(newUser)!;
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
}