using MusicCollection.BusinessLogic.Repositories;
using MusicCollection.BusinessLogic.Repositories.Auth;
using MusicCollection.BusinessLogic.Repositories.Database;

namespace MusicCollection.BusinessLogic.Services.AuthService;

public class AuthService : IAuthService
{
    public Task RegisterUser(UserStorageElement user)
    {
        throw new NotImplementedException();
    }

    public Task GetUser(Guid id)
    {
        throw new NotImplementedException();
    }
}