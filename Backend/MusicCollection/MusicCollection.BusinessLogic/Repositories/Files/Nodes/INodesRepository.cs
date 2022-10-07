using MusicCollection.Api.Dto.FileSystem;

namespace MusicCollection.BusinessLogic.Repositories.Files.Nodes;

public interface INodesRepository
{
    Task CreateAsync(FileSystemNode node);
    Task CreateManyAsync(FileSystemNode[] nodes);
    Task<FileSystemNode[]> ReadAllFilesAsync(Guid parentId, bool withPages = true, int skip = 0, int take = 50);
    Task<FileSystemNode> ReadAsync(Guid id);
    Task<FileSystemNode?> TryReadAsync(Guid id);
}