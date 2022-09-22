using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MusicCollection.Api.Dto.Exceptions;
using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.BusinessLogic.Repositories.Database;

namespace MusicCollection.BusinessLogic.Repositories.Files;

public class NodesRepository : INodesRepository
{
    private readonly DatabaseContext databaseContext;

    public NodesRepository(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }


    public async Task CreateAsync(FileSystemNode node)
    {
        await databaseContext.NodesStorage.AddAsync(ToStorageElement(node));
        await databaseContext.SaveChangesAsync();
    }
    
    public async Task<FileSystemNode[]> ReadAllFilesAsync(Guid parentId)
    {
        return await databaseContext.NodesStorage
            .Where(node => node.ParentId == parentId)
            .Select(node => ToModel(node))
            .ToArrayAsync();
    }

    public async Task<FileSystemNode> ReadAsync(Guid id)
    {
        var requiredNode = await databaseContext.NodesStorage.FirstAsync(node => node.Id == id);
        if (requiredNode is null) throw new FileSystemNodeNotFoundException();
        return ToModel(requiredNode);
    }

    public async Task<FileSystemNode?> TryReadAsync(Guid id)
    {
        return ToModel(await databaseContext.NodesStorage.FirstAsync(node => node.Id == id));
    }

    private FileSystemNode ToModel(NodeStorageElement node)
    {
        return new FileSystemNode()
        {
            Id = node.Id,
            ParentId = node.ParentId,
            Type = node.Type,
            Tags = node.Tags
        };
    }

    private NodeStorageElement ToStorageElement(FileSystemNode node)
    {
        return new NodeStorageElement()
        {
            Id = node.Id,
            ParentId = node.ParentId,
            Type = node.Type,
            Tags = node.Tags
        };
    }
}