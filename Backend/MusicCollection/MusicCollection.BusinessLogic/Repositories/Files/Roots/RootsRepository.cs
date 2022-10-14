using Microsoft.EntityFrameworkCore;
using MusicCollection.Api.Dto.Exceptions;
using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.BusinessLogic.Repositories.Database;

namespace MusicCollection.BusinessLogic.Repositories.Files.Roots;

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

    public async Task<FileSystemRoot[]> ReadAllAsync()
    {
        var result = await databaseContext.RootsStorage.ToArrayAsync();
        return result.Select(ToModel).ToArray();
    }

    public async Task CreateAsync(FileSystemRoot root)
    {
        await databaseContext.RootsStorage.AddAsync(ToStorageElement(root));
        await databaseContext.SaveChangesAsync();
    }

    private static FileSystemRoot ToModel(RootStorageElement root)
    {
        return new FileSystemRoot()
        {
            Id = root.Id,
            Path = root.Path
        };
    }

    private static RootStorageElement ToStorageElement(FileSystemRoot root)
    {
        return new RootStorageElement()
        {
            Id = root.Id,
            Path = root.Path
        };
    }
}