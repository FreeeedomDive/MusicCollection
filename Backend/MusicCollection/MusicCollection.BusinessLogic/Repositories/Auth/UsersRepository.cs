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

    public async Task<User?> TryReadAsync(Guid id)
    {
        return ToModel( await databaseContext.UsersStorage.FirstAsync(user => user.Id == id));
    }

    public async Task CreateOrUpdateAsync(User user)
    {
        var requiredUser = await TryReadAsync(user.Id);
        if (requiredUser is null)
        {
            await CreateAsync(user);
        }
        else
        {
            await UpdateAsync(user);
        }
    }

    public async Task CreateAsync(User user)
    {
        await databaseContext.UsersStorage.AddAsync(ToStorageElement(user));
        await databaseContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        var requiredUser = await databaseContext.UsersStorage.FirstAsync(u => u.Id == user.Id);
        requiredUser.Login = user.Login;
        requiredUser.Password = user.Password;
        await databaseContext.SaveChangesAsync();
    }

    public async Task<bool> TryDeleteAsync(User user)
    {
        var requiredUser = await databaseContext.UsersStorage.FirstAsync(u => u.Id == user.Id);
        if (requiredUser is null) return false;

        databaseContext.Remove(requiredUser);
        return true;
    }

    private static UserStorageElement ToStorageElement(User user)
    {
        return new UserStorageElement()
        {
            Login = user.Login,
            Password = user.Password
        };
    }
    
    private static User ToModel(UserStorageElement user)
    {
        return new User()
        {
            Login = user.Login,
            Password = user.Password
        };
    }
}