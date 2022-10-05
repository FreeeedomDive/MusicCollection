using Microsoft.EntityFrameworkCore;
using MusicCollection.Api.Dto.Auth;
using MusicCollection.Api.Dto.Exceptions;
using MusicCollection.BusinessLogic.Repositories.Database;

namespace MusicCollection.BusinessLogic.Repositories.Auth;

public class UsersRepository : IUsersRepository
{
    private readonly DatabaseContext databaseContext;

    public UsersRepository(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }

    public async Task<User?> FindAsync(string login)
    {
        return ToModel(await databaseContext.UsersStorage.FirstOrDefaultAsync(user => user.Login == login));
    }

    public async Task<User?> FindAsync(string login, string encryptedPassword)
    {
        return ToModel(await databaseContext.UsersStorage.FirstOrDefaultAsync(user =>
            user.Login == login
            && user.Password == encryptedPassword)
        );
    }

    public async Task<User?> TryReadAsync(Guid id)
    {
        return ToModel(await databaseContext.UsersStorage.FirstAsync(user => user.Id == id));
    }

    public async Task<User> CreateAsync(string login, string encryptedPassword)
    {
        var newUser = new UserStorageElement
        {
            Id = new Guid(),
            Login = login,
            Password = encryptedPassword
        };
        await databaseContext.UsersStorage.AddAsync(newUser);
        await databaseContext.SaveChangesAsync();
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