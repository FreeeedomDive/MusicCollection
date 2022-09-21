using Microsoft.EntityFrameworkCore;
using MusicCollection.Api.Dto.Exceptions;
using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.BusinessLogic.Repositories.Database;

namespace MusicCollection.BusinessLogic.Repositories.Files;

public class RootsRepository : IRootsRepository
{
    private readonly DatabaseContext databaseContext;

    public RootsRepository(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }

    public async Task<FileSystemRoot> ReadAsync(Guid id)
    {
        var requiredRoot = await databaseContext.RootsStorage.FirstAsync(root => root.Id == id);
        if (requiredRoot is null) throw new RootNotFoundException();
        return ToModel(requiredRoot);
    }

    public async Task<FileSystemRoot> TryReadAsync(Guid id)
    {
        return ToModel(await databaseContext.RootsStorage.FirstAsync(root => root.Id == id));
    }

    public async Task CreateAsync(FileSystemRoot root)
    {
        await databaseContext.RootsStorage.AddAsync(ToStorageElement(root));
        await databaseContext.SaveChangesAsync();
    }

    public async Task<bool> TryDeleteAsync(FileSystemRoot root)
    {
        var requiredRoot = await databaseContext.RootsStorage.FirstAsync(r => r.Id == root.Id);
        if (requiredRoot is null)
        {
            return false;
        }

        databaseContext.Remove(requiredRoot);
        await databaseContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<FileSystemRoot>> ReadManyAsync(Guid[] ids)
    {
        return await databaseContext.RootsStorage
            .Where(root => ids.Contains(root.Id))
            .Select(root => ToModel(root))
            .ToListAsync();
    }
    
    private FileSystemRoot ToModel(RootStorageElement root)
    {
        return new FileSystemRoot()
        {
            Id = root.Id,
            Path = root.Path
        };
    }

    private RootStorageElement ToStorageElement(FileSystemRoot root)
    {
        return new RootStorageElement()
        {
            Id = root.Id,
            Path = root.Path
        };
    }
}