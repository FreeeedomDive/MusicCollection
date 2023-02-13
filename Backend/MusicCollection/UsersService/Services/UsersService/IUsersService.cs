using MusicCollection.Api.Dto.Interfaces;
using MusicCollection.Api.Dto.Users;

namespace MusicCollection.UsersService.Services.UsersService;

public interface IUsersService : IMusicCollectionLogicService
{
    Task<User> LoginAsync(AuthCredentials authCredentials);
    Task<User> RegisterAsync(AuthCredentials authCredentials);
}