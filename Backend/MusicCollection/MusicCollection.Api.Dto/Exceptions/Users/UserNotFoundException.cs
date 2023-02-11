using MusicCollection.Api.Dto.Exceptions.Api;

namespace MusicCollection.Api.Dto.Exceptions.Users;

public class UserNotFoundException : MusicCollectionApiNotFoundException
{
    public UserNotFoundException(Guid id) : base($"User {id} not found")
    {
    }
    
    public UserNotFoundException(string login) : base($"User {login} not found")
    {
    }
}