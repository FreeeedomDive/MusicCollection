using MusicCollection.BusinessLogic.Repositories.Auth;

namespace MusicCollection.BusinessLogic.Services.AuthService;

public interface IAuthService
{
    Task RegisterUser(UserStorageElement user);
    Task GetUser(Guid id);
}