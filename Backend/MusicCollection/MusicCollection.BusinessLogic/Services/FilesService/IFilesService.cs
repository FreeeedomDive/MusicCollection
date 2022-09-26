using MusicCollection.Api.Dto.FileSystem;

namespace MusicCollection.BusinessLogic.Services.FilesService;

public interface IFilesService
{
    Task<FileSystemNode[]> ReadAllFiles(Guid parentId);
    Task<FileSystemRoot[]> ReadAllRoots();
    Task<FileSystemNode> ReadNodeAsync(Guid id);
    Task<FileSystemNode?> TryReadNodeAsync(Guid id);
    Task<FileSystemRoot> ReadRootAsync(Guid id);
}