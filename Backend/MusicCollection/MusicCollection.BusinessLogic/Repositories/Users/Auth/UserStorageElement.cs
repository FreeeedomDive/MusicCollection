using SqlRepositoryBase.Core.Models;

namespace MusicCollection.BusinessLogic.Repositories.Auth;

public class UserStorageElement : SqlStorageElement
{
    public string Login { get; set; }
    public string Password { get; set; }
}