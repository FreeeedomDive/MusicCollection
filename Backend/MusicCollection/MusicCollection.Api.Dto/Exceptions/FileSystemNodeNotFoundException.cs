namespace MusicCollection.Api.Dto.Exceptions;

public class FileSystemNodeNotFoundException : MusicCollectionApiExceptionBase
{
    public Guid Id { get; set; }

    public FileSystemNodeNotFoundException(Guid id)
    {
        Id = id;
    }
}