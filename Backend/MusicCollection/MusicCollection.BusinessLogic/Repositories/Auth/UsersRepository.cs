using MusicCollection.BusinessLogic.Repositories.Database;

namespace MusicCollection.BusinessLogic.Repositories.Auth;

public class UsersRepository : IUsersRepository
{
    private readonly DatabaseContext databaseContext;

    public UsersRepository(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }

    public async Task<UserStorageElement> GetUser(string login)
    {
        return await databaseContext.UsersStorage.FindAsync(login);
    }

    public async Task AddUser(UserStorageElement user)
    {
        await databaseContext.AddAsync(user);
        await databaseContext.SaveChangesAsync();
    }
}