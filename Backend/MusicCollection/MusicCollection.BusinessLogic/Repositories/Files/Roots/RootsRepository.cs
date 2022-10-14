using DatabaseCore.Exceptions;
using DatabaseCore.Repository;
using MusicCollection.Api.Dto.Exceptions;
using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.BusinessLogic.Repositories.Database;

namespace MusicCollection.BusinessLogic.Repositories.Files.Roots;

public class RootsRepository : SqlRepository<RootStorageElement>, IRootsRepository
{
    public RootsRepository(DatabaseContext databaseContext) : base(databaseContext, databaseContext.RootsStorage)
    {
    }

    public new async Task<FileSystemRoot> ReadAsync(Guid id)
    {
        try
        {
            var result = await base.ReadAsync(id);
            return ToModel(result);
        }
        catch (SqlEntityNotFoundException)
        {
            throw new RootNotFoundException(id);
        }
    }

    public new async Task<FileSystemRoot[]> ReadAllAsync()
    {
        var result = await base.ReadAllAsync();
        return result.Select(ToModel).ToArray();
    }

    public async Task CreateAsync(FileSystemRoot root)
    {
        await CreateAsync(ToStorageElement(root));
    }

    private static FileSystemRoot ToModel(RootStorageElement root)
    {
        return new FileSystemRoot
        {
            Id = root.Id,
            Path = root.Path
        };
    }

    private static RootStorageElement ToStorageElement(FileSystemRoot root)
    {
        return new RootStorageElement
        {
            Id = root.Id,
            Path = root.Path
        };
    }
}