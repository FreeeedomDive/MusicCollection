using SqlRepositoryBase.Core.Models;

namespace MusicCollection.UserService.Repositories.Users.Personalization;

public class UserSettingsStorageElement : SqlStorageElement
{
    public bool Shuffle { get; set; }
}