namespace MusicCollection.BusinessLogic.Repositories.Files;

public interface INodesRepository
{
    Task CreateNode(NodeStorageElement node);
    Task<List<NodeStorageElement>> GetFilesInDirectory(Guid parentId);
    Task<NodeStorageElement> GetFile(Guid id);
}