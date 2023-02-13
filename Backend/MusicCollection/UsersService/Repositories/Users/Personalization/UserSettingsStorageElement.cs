using SqlRepositoryBase.Core.Models;

namespace MusicCollection.BusinessLogic.Repositories.Users.Personalization;

public class UserSettingsStorageElement : SqlStorageElement
{
    public bool Shuffle { get; set; }
}