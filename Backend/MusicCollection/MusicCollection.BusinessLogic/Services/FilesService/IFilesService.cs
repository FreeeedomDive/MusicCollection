using MusicCollection.Api.Dto.FileSystem;

namespace MusicCollection.BusinessLogic.Services.FilesService;

public interface IFilesService
{
    Task<FileSystemNode[]> ReadDirectoryAsync(Guid directoryId);
    Task<Guid[]> ReadAllFilesFromDirectoryAsync(Guid directoryId);
    Task<FileSystemRoot[]> ReadAllRoots();
    Task<FileSystemNode> ReadNodeAsync(Guid id);
    Task<FileSystemNode?> TryReadNodeAsync(Guid id);
    Task<FileSystemRoot> ReadRootAsync(Guid id);
}