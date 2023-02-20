using MusicCollection.Api.Dto.Interfaces;
using MusicCollection.Api.Dto.Users;

namespace MusicCollection.UserService.Services.UsersService;

public interface IUsersService : IMusicCollectionLogicService
{
    Task<User> LoginAsync(AuthCredentials authCredentials);
    Task<User> RegisterAsync(AuthCredentials authCredentials);
    Task<UserSettings> ReadOrCreateSettingsAsync(Guid userId);
    Task UpdateSettingsAsync(Guid userId, UserSettings userSettings);
}