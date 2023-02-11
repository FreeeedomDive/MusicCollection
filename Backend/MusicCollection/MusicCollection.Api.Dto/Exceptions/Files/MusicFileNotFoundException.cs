using MusicCollection.Api.Dto.Exceptions.Api;

namespace MusicCollection.Api.Dto.Exceptions.Files;

public class MusicFileNotFoundException : MusicCollectionApiNotFoundException
{
    public MusicFileNotFoundException(string path) : base($"File {path} not found")
    {
    }
}