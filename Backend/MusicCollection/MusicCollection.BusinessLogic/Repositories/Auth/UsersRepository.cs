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

    public async Task<User> ReadAsync(Guid id)
    {
        var user = await databaseContext.UsersStorage.FirstAsync(user => user.Id == id);
        if (user is null) throw new UserNotFoundException();
        return ToModel(user);
    }

    public async Task<User> ReadAsync(string login, string encryptedPassword)
    {
        return ToModel(await databaseContext.UsersStorage.FirstAsync(user => user.Login == login
                                                                             && user.Password == encryptedPassword));
    }

    public async Task<User?> TryReadAsync(Guid id)
    {
        return ToModel( await databaseContext.UsersStorage.FirstAsync(user => user.Id == id));
    }

    public async Task CreateAsync(string login, string encryptedPassword)
    {
        await databaseContext.UsersStorage.AddAsync(new UserStorageElement()
        {
            Login = login,
            Password = encryptedPassword
        });
        await databaseContext.SaveChangesAsync();
    }

    public async Task<Guid> CreateAsync(User user)
    {
        await databaseContext.UsersStorage.AddAsync(ToStorageElement(user));
        await databaseContext.SaveChangesAsync();
        return user.Id;
    }

    private static UserStorageElement ToStorageElement(User user)
    {
        return new UserStorageElement
        {
            Login = user.Login,
            Password = user.Password
        };
    }
    
    private static User ToModel(UserStorageElement user)
    {
        return new User
        {
            Login = user.Login,
        };
    }
}