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

    public async Task CreateOrUpdateAsync(FileSystemNode node)
    {
        var requiredNode = await TryReadAsync(node.Id);
        if (requiredNode is null)
        {
            await CreateAsync(node);
        }
        else
        {
            await UpdateAsync(node);
        }
    }

    public async Task UpdateAsync(FileSystemNode node)
    {
        var requiredNode = await databaseContext.NodesStorage.FirstAsync(n => n.Id == node.Id);
        requiredNode.Tags = node.Tags;
        await databaseContext.SaveChangesAsync();
    }

    public async Task<List<FileSystemNode>> ReadAllFilesAsync(Guid parentId)
    {
        return await databaseContext.NodesStorage
            .Where(node => node.ParentId == parentId)
            .Select(node => ToModel(node))
            .ToListAsync();
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

    public async Task<bool> TryDeleteAsync(FileSystemNode node)
    {
        var requiredNode = await databaseContext.NodesStorage.FirstAsync(n => n.Id == node.Id);
        if (requiredNode is null)
        {
            return false;
        }

        databaseContext.NodesStorage.Remove(requiredNode);
        await databaseContext.SaveChangesAsync();
        return true;
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