using MusicCollection.Api.Dto.Exceptions.Files;

namespace MusicCollection.BusinessLogic.Repositories.Files.Music;

public class MusicFilesRepository : IMusicFilesRepository
{
    public Task<byte[]> ReadFileAsync(string path)
    {
        if (!File.Exists(path))
        {
            throw new MusicFileNotFoundException(path);
        }

        return File.ReadAllBytesAsync(path);
    }

    public FileStream ReadFileAsStream(string path)
    {
        if (!File.Exists(path))
        {
            throw new MusicFileNotFoundException(path);
        }

        return File.OpenRead(path);
    }
}