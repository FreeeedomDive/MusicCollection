namespace MusicCollection.Api.Dto.Exceptions;

public class DirectoryNotFoundException : Exception
{
    public DirectoryNotFoundException(string path) : base($"Directory {path} not found")
    {
        Path = path;
    }

    public string Path { get; }
}