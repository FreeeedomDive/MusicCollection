using MusicCollection.Api.Dto.FileSystem;

namespace MusicCollection.BusinessLogic.Repositories.Files;

public interface INodesRepository
{
    Task CreateAsync(FileSystemNode node);
    Task CreateOrUpdateAsync(FileSystemNode node);
    Task UpdateAsync(FileSystemNode node);
    Task<List<FileSystemNode>> ReadAllFilesAsync(Guid parentId);
    Task<FileSystemNode> ReadAsync(Guid id);
    Task<FileSystemNode?> TryReadAsync(Guid id);
    Task<List<FileSystemNode>> ReadManyAsync(Guid[] ids);
    Task<bool> TryDeleteAsync(FileSystemNode node);
}