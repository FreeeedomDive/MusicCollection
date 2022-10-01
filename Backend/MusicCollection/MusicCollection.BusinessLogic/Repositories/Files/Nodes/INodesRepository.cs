using MusicCollection.Api.Dto.FileSystem;

namespace MusicCollection.BusinessLogic.Repositories.Files.Nodes;

public interface INodesRepository
{
    Task CreateAsync(FileSystemNode node);
    Task<FileSystemNode[]> ReadAllFilesAsync(Guid parentId);
    Task<FileSystemNode> ReadAsync(Guid id);
    Task<FileSystemNode?> TryReadAsync(Guid id);
}