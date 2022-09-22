namespace MusicCollection.Api.Dto.Exceptions;

public class RootNotFoundException : MusicCollectionApiExceptionBase
{
    public Guid Id { get; set; }
    public RootNotFoundException()
    {
        
    }

    public RootNotFoundException(Guid id)
    {
        Id = id;
    }
}