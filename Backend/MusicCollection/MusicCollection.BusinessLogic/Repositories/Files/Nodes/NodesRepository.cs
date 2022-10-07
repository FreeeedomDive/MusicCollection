﻿using Microsoft.EntityFrameworkCore;
using MusicCollection.Api.Dto.Exceptions;
using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.BusinessLogic.Repositories.Database;

namespace MusicCollection.BusinessLogic.Repositories.Files.Nodes;

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

    public async Task CreateManyAsync(FileSystemNode[] nodes)
    {
        var storageElements = nodes.Select(ToStorageElement);
        await databaseContext.NodesStorage.AddRangeAsync(storageElements);
        await databaseContext.SaveChangesAsync();
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

        var requiredNodesQueryable = databaseContext.NodesStorage
            .Where(node => node.ParentId == parentId)
            .OrderByDescending(x => x.Type)
            .ThenBy(x => x.Path);
        var result = withPages
            ? await requiredNodesQueryable
                .Skip(skip)
                .Take(take)
                .ToArrayAsync()
            : await requiredNodesQueryable.ToArrayAsync();

        return result.Select(ToModel).ToArray();
    }

    public async Task<FileSystemNode> ReadAsync(Guid id)
    {
        var requiredNode = await databaseContext.NodesStorage.FirstOrDefaultAsync(node => node.Id == id);
        if (requiredNode is null) throw new FileSystemNodeNotFoundException(id);
        return ToModel(requiredNode);
    }

    public async Task<FileSystemNode?> TryReadAsync(Guid id)
    {
        return ToModel(await databaseContext.NodesStorage.FirstAsync(node => node.Id == id));
    }

    private static FileSystemNode ToModel(NodeStorageElement node)
    {
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