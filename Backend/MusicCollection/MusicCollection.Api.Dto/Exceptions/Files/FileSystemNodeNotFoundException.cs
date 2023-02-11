using MusicCollection.Api.Dto.Exceptions.Api;

namespace MusicCollection.Api.Dto.Exceptions.Files;

public class FileSystemNodeNotFoundException : MusicCollectionApiNotFoundException
{
    public FileSystemNodeNotFoundException(Guid id) : base($"Node {id} not found")
    {
    }
}