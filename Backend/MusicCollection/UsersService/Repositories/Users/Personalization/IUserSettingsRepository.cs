using MusicCollection.Api.Dto.Interfaces;
using MusicCollection.Api.Dto.Users;

namespace MusicCollection.BusinessLogic.Repositories.Users.Personalization;

public interface IUserSettingsRepository : IMusicCollectionRepository
{
    Task<UserSettings> ReadOrCreateAsync(Guid userId);
    Task UpdateAsync(Guid userId, UserSettings userSettings);
}