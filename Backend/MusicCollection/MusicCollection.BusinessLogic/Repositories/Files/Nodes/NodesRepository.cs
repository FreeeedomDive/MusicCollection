using Microsoft.EntityFrameworkCore;
using MusicCollection.Api.Dto.Exceptions.Files;
using MusicCollection.Api.Dto.FileSystem;
using SqlRepositoryBase.Core.Exceptions;
using SqlRepositoryBase.Core.Repository;

namespace MusicCollection.BusinessLogic.Repositories.Files.Nodes;

public class NodesRepository : INodesRepository
{
    public NodesRepository(ISqlRepository<NodeStorageElement> sqlRepository)
    {
        this.sqlRepository = sqlRepository;
    }

    public async Task CreateAsync(FileSystemNode node)
    {
        await sqlRepository.CreateAsync(ToStorageElement(node));
    }

    public async Task CreateManyAsync(FileSystemNode[] nodes)
    {
        await sqlRepository.CreateAsync(nodes.Select(ToStorageElement));
    }

    public async Task<FileSystemNode[]> ReadAllFilesAsync(
        Guid parentId,
        bool withPages = true,
        int skip = 0,
        int take = 50,
        bool includeHidden = false
    )
    {
        if ((await ReadAsync(parentId)).Type == NodeType.File)
        {
            throw new ReadFilesFromNonDirectoryException(parentId);
        }

        var requiredNodesQueryable = sqlRepository
                                     .BuildCustomQuery()
                                     .Where(node => node.ParentId == parentId && !node.Hidden)
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

    public async Task<FileSystemNode> ReadAsync(Guid id)
    {
        try
        {
            var result = await sqlRepository.ReadAsync(id);
            return ToModel(result)!;
        }
        catch (SqlEntityNotFoundException)
        {
            throw new FileSystemNodeNotFoundException(id);
        }
    }

    public async Task<FileSystemNode[]> ReadManyAsync(Guid[] ids)
    {
        return (await sqlRepository.ReadAsync(ids)).Select(ToModel).ToArray()!;
    }

    public async Task<FileSystemNode?> TryReadAsync(Guid id)
    {
        var result = await sqlRepository.TryReadAsync(id);
        return ToModel(result);
    }

    public async Task HideNodeAsync(Guid id)
    {
        await sqlRepository.UpdateAsync(id, x => x.Hidden = true);
    }

    public async Task DeleteAsync(Guid id)
    {
        await sqlRepository.DeleteAsync(id);
    }

    public async Task DeleteManyAsync(Guid[] ids)
    {
        await sqlRepository.DeleteAsync(ids);
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
            Path = node.Path,
            Hidden = node.Hidden,
        };
    }

    private static NodeStorageElement ToStorageElement(FileSystemNode node)
    {
        return new NodeStorageElement
        {
            Id = node.Id,
            ParentId = node.ParentId,
            Type = node.Type,
            Path = node.Path,
            Hidden = node.Hidden,
        };
    }

    private readonly ISqlRepository<NodeStorageElement> sqlRepository;
}