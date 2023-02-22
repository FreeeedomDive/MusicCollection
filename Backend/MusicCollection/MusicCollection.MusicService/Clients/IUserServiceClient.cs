using MusicCollection.Api.Dto.Interfaces;
using MusicCollection.Api.Dto.Users;

namespace MusicCollection.MusicService.Clients;

public interface IUserServiceClient : IMusicCollectionLogicService
{
    Task<UserSettings> ReadOrCreateUserSettingsAsync(Guid userId);
    Task UpdateAsync(Guid userId, UserSettings userSettings);
}