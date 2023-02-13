using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.Api.Dto.Interfaces;

namespace MusicCollection.MusicService.Services.FilesService;

public interface IFilesService : IMusicCollectionLogicService
{
    Task<FileSystemNode[]> ReadDirectoryAsync(Guid directoryId, int skip = 0, int take = 50);
    Task<Guid[]> ReadAllFilesFromDirectoryAsync(Guid directoryId);
    Task<FileSystemRoot[]> ReadAllRoots();
    Task<FileSystemNode> ReadNodeAsync(Guid id);
    Task<FileSystemNode[]> ReadManyNodesAsync(Guid[] ids);
    Task<FileSystemNode?> TryReadNodeAsync(Guid id);
    Task<FileSystemRoot> ReadRootAsync(Guid id);
    Task<Guid> CreateRootWithIndexAsync(string name, string path);
    Task<byte[]> ReadFileContentAsync(Guid id);
    Task<(FileStream, string)> ReadFileContentAsStreamAsync(Guid id);
    Task HideNodeAsync(Guid id);
}