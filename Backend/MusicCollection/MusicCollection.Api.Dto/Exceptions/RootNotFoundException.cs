namespace MusicCollection.Api.Dto.Exceptions;

public class RootNotFoundException : MusicCollectionApiExceptionBase
{
    public RootNotFoundException(Guid id) : base($"Root {id} not found")
    {
    }
}