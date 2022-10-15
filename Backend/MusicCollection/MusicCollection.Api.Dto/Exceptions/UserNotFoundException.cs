namespace MusicCollection.Api.Dto.Exceptions;

public class UserNotFoundException : MusicCollectionApiExceptionBase
{
    public UserNotFoundException(Guid id) : base($"User {id} not found")
    {
    }
    
    public UserNotFoundException(string login) : base($"User {login} not found")
    {
    }
}