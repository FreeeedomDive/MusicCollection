using MusicCollection.Api.Dto.FileSystem;

namespace MusicCollection.BusinessLogic.Services.FilesService;

public interface IFilesService
{
    Task<List<FileSystemNode>> ReadAllFiles(Guid parentId);
    Task<FileSystemNode> ReadNodeAsync(Guid id);
    Task<FileSystemNode?> TryReadNodeAsync(Guid id);
    Task<FileSystemRoot> ReadRootAsync(Guid id);
    Task<FileSystemRoot?> TryReadRootAsync(Guid id);
    Task UpdateNodeAsync(FileSystemNode node);
    Task TryDeleteNodeAsync(FileSystemNode node);
    Task<bool> TryDeleteRootAsync(FileSystemRoot root);
}