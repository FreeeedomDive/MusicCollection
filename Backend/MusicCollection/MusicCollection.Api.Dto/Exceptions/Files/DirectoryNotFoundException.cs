using MusicCollection.Api.Dto.Exceptions.Api;

namespace MusicCollection.Api.Dto.Exceptions.Files;

public class DirectoryNotFoundException : MusicCollectionApiNotFoundException
{
    public DirectoryNotFoundException(string path) : base($"Directory {path} not found")
    {
    }
}