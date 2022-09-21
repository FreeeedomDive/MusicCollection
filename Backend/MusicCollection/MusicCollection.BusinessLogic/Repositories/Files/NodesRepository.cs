using Microsoft.EntityFrameworkCore;
using MusicCollection.BusinessLogic.Repositories.Database;

namespace MusicCollection.BusinessLogic.Repositories.Files;

public class NodesRepository : INodesRepository
{
    private readonly DatabaseContext databaseContext;

    public NodesRepository(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }

    public async Task CreateNode(NodeStorageElement node)
    {
        var newNode = new NodeStorageElement()
        {
            Id = node.Id,
            ParentId = node.Id,
            Type = node.Type
        };
        await databaseContext.NodesStorage.AddAsync(newNode);
        await databaseContext.SaveChangesAsync();
    }

    public async Task<List<NodeStorageElement>> GetFilesInDirectory(Guid parentId)
    {
        return await databaseContext.NodesStorage.Where(node => node.ParentId == parentId).ToListAsync();
    }

    public async Task<NodeStorageElement> GetFile(Guid id)
    {
        return await databaseContext.NodesStorage.FindAsync(id);
    }
}