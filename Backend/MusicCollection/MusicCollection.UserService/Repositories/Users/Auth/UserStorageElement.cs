using SqlRepositoryBase.Core.Models;

namespace MusicCollection.UserService.Repositories.Users.Auth;

public class UserStorageElement : SqlStorageElement
{
    public string Login { get; set; }
    public string Password { get; set; }
}