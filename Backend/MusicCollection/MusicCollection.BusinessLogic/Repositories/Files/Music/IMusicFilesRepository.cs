namespace MusicCollection.BusinessLogic.Repositories.Files.Music;

public interface IMusicFilesRepository : IMusicCollectionRepository
{
    Task<byte[]> ReadFileAsync(string path);
    FileStream ReadFileAsStream(string path);
}