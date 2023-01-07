using MusicCollection.Api.Dto.Users;

namespace MusicCollection.BusinessLogic.Services.UsersService;

public interface IUsersService : IMusicCollectionLogicService
{
    Task<User> LoginAsync(AuthCredentials authCredentials);
    Task<User> RegisterAsync(AuthCredentials authCredentials);
}