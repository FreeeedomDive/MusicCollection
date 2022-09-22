using MusicCollection.Api.Dto.FileSystem;

namespace MusicCollection.BusinessLogic.Services.FilesService;

public interface IFileService
{
    Task<List<FileSystemNode>> ReadAllFiles(Guid parentId);
    Task<FileSystemNode> ReadNodeAsync(Guid id);
    Task<FileSystemNode?> TryReadNodeAsync(Guid id);
    Task UpdateNodeAsync(FileSystemNode node);
    Task<List<FileSystemNode>> ReadManyNodesAsync(Guid[] ids);
    Task TryDeleteNodeAsync(FileSystemNode node);
}