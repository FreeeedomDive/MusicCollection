namespace MusicCollection.Api.Dto.Exceptions;

public class MusicFileNotFoundException : MusicCollectionApiExceptionBase
{
    public MusicFileNotFoundException(string path) : base($"File {path} not found")
    {
    }
}