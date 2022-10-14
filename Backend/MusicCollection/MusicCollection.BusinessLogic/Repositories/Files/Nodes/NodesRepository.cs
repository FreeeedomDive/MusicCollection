﻿using DatabaseCore.Exceptions;
using DatabaseCore.Repository;
using Microsoft.EntityFrameworkCore;
using MusicCollection.Api.Dto.Exceptions;
using MusicCollection.Api.Dto.FileSystem;

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
        await sqlRepository.CreateManyAsync(nodes.Select(ToStorageElement));
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

        var requiredNodesQueryable = sqlRepository
            .BuildCustomQuery()
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

    public async Task<FileSystemNode?> TryReadAsync(Guid id)
    {
        var result = await sqlRepository.TryReadAsync(id);
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

    private readonly ISqlRepository<NodeStorageElement> sqlRepository;
}