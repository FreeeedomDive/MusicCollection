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

    public async Task<User> FindAsync(string login, string encryptedPassword)
    {
        return ToModel(await databaseContext.UsersStorage.FirstOrDefaultAsync(user => user.Login == login
            && user.Password == encryptedPassword) ?? throw new UserNotFoundException());
    }

    public async Task<User?> TryReadAsync(Guid id)
    {
        return ToModel( await databaseContext.UsersStorage.FirstAsync(user => user.Id == id));
    }

    public async Task CreateAsync(string login, string encryptedPassword)
    {
        await databaseContext.UsersStorage.AddAsync(new UserStorageElement()
        {
            Id = new Guid(),
            Login = login,
            Password = encryptedPassword
        });
        await databaseContext.SaveChangesAsync();
    }

    private static User ToModel(UserStorageElement user)
    {
        return new User
        {
            Login = user.Login,
        };
    }
}