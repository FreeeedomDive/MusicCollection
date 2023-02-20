using MusicCollection.Api.Dto.Interfaces;

namespace MusicCollection.MusicService.Repositories.Files.Music;

public interface IMusicFilesRepository : IMusicCollectionRepository
{
    Task<byte[]> ReadFileAsync(string path);
    FileStream ReadFileAsStream(string path);
}