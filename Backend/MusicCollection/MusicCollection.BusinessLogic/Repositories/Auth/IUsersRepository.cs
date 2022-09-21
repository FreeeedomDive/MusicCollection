namespace MusicCollection.BusinessLogic.Repositories.Auth;

public interface IUsersRepository
{
    Task<UserStorageElement> GetUser(string login);
    Task AddUser(UserStorageElement user);
}