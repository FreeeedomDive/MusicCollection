using MusicCollection.Api.Dto.Users;
using SqlRepositoryBase.Core.Repository;

namespace MusicCollection.BusinessLogic.Repositories.Users.Personalization;

public class UserSettingsRepository : IUserSettingsRepository
{
    public UserSettingsRepository(ISqlRepository<UserSettingsStorageElement> sqlRepository)
    {
        this.sqlRepository = sqlRepository;
    }

    public async Task<UserSettings> ReadOrCreateAsync(Guid userId)
    {
        var existing = await sqlRepository.TryReadAsync(userId);
        if (existing != null)
        {
            return ToModel(existing);
        }

        var newSettings = UserSettings.CreateDefault();
        await sqlRepository.CreateAsync(ToStorageElement(userId, newSettings));

        return newSettings;
    }

    public async Task UpdateAsync(Guid userId, UserSettings userSettings)
    {
        await sqlRepository.UpdateAsync(userId, x =>
        {
            // update every property
            x.Shuffle = userSettings.Shuffle;
        });
    }

    private static UserSettings ToModel(UserSettingsStorageElement storageElement)
    {
        return new UserSettings
        {
            Shuffle = storageElement.Shuffle
        };
    }

    private static UserSettingsStorageElement ToStorageElement(Guid userId, UserSettings model)
    {
        return new UserSettingsStorageElement
        {
            Id = userId,
            Shuffle = model.Shuffle
        };
    }

    private readonly ISqlRepository<UserSettingsStorageElement> sqlRepository;
}