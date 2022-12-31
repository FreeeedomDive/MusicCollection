using MusicCollection.Api.Dto.FileSystem;

namespace MusicCollection.BusinessLogic.Repositories.Files.Nodes;

public interface INodesRepository : IMusicCollectionRepository
{
    Task CreateAsync(FileSystemNode node);
    Task CreateManyAsync(FileSystemNode[] nodes);

    Task<FileSystemNode[]> ReadAllFilesAsync(Guid parentId, bool withPages = true, int skip = 0, int take = 50,
        bool includeHidden = false);

    Task<FileSystemNode> ReadAsync(Guid id);
    Task<FileSystemNode[]> ReadManyAsync(Guid[] ids);
    Task<FileSystemNode?> TryReadAsync(Guid id);

    Task HideNodeAsync(Guid id);

    Task DeleteAsync(Guid id);
    Task DeleteManyAsync(Guid[] ids);
}