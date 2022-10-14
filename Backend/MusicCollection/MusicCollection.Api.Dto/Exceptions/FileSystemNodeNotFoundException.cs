namespace MusicCollection.Api.Dto.Exceptions;

public class FileSystemNodeNotFoundException : MusicCollectionApiExceptionBase
{
    public FileSystemNodeNotFoundException(Guid id) : base($"Node {id} not found")
    {
    }
}