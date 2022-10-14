using DatabaseCore.Exceptions;
using DatabaseCore.Repository;
using Microsoft.EntityFrameworkCore;
using MusicCollection.Api.Dto.Exceptions;
using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.BusinessLogic.Repositories.Database;

namespace MusicCollection.BusinessLogic.Repositories.Files.Nodes;

public class NodesRepository : SqlRepository<NodeStorageElement>, INodesRepository
{
    public NodesRepository(DatabaseContext databaseContext): base(databaseContext, databaseContext.NodesStorage)
    {
    }

    public async Task CreateAsync(FileSystemNode node)
    {
        await CreateAsync(ToStorageElement(node));
    }

    public async Task CreateManyAsync(FileSystemNode[] nodes)
    {
        await CreateManyAsync(nodes.Select(ToStorageElement));
    }

    public async Task<FileSystemNode[]> ReadAllFilesAsync(
        Guid parentId,
        bool withPages = true,
        int skip = 0,
        int take = 50
    )
    {
        if ((await ReadAsync(parentId)).Type == NodeType.File)
        {
            throw new ReadFilesFromNonDirectoryException(parentId);
        }

        var requiredNodesQueryable = BuildCustomQuery()
            .Where(node => node.ParentId == parentId)
            .OrderByDescending(x => x.Type)
            .ThenBy(x => x.Path);
        var result = withPages
            ? await requiredNodesQueryable
                .Skip(skip)
                .Take(take)
                .ToArrayAsync()
            : await requiredNodesQueryable.ToArrayAsync();

        return result.Select(ToModel).ToArray()!;
    }

    public new async Task<FileSystemNode> ReadAsync(Guid id)
    {
        try
        {
            var result = await base.ReadAsync(id);
            return ToModel(result)!;
        }
        catch (SqlEntityNotFoundException)
        {
            throw new FileSystemNodeNotFoundException(id);
        }
    }

    public new async Task<FileSystemNode?> TryReadAsync(Guid id)
    {
        var result = await base.TryReadAsync(id);
        return ToModel(result);
    }

    private static FileSystemNode? ToModel(NodeStorageElement? node)
    {
        if (node == null)
        {
            return null;
        }

        return new FileSystemNode
        {
            Id = node.Id,
            ParentId = node.ParentId,
            Type = node.Type,
            Path = node.Path
        };
    }

    private static NodeStorageElement ToStorageElement(FileSystemNode node)
    {
        return new NodeStorageElement
        {
            Id = node.Id,
            ParentId = node.ParentId,
            Type = node.Type,
            Path = node.Path
        };
    }
}