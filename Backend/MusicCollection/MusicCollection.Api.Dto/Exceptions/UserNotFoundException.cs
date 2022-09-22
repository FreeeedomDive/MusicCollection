namespace MusicCollection.Api.Dto.Exceptions;

public class UserNotFoundException : MusicCollectionApiExceptionBase
{
    public Guid Id { get; }
    public UserNotFoundException() 
    {
        
    }

    public UserNotFoundException(Guid id)
    {
        Id = id;
    }
}