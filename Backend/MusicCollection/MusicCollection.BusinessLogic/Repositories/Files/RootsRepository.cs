using MusicCollection.BusinessLogic.Repositories.Database;

namespace MusicCollection.BusinessLogic.Repositories.Files;

public class RootsRepository : IRootsRepository
{
    private readonly DatabaseContext databaseContext;

    public RootsRepository(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }

    public async Task<RootStorageElement> Get(Guid id)
    {
        return await databaseContext.RootsStorage.FindAsync(id);
    }

    public async Task CreateRoot(Guid id, string path)
    {
        await databaseContext.RootsStorage.AddAsync(new RootStorageElement()
        {
            Id = id,
            Path = path
        });
        await databaseContext.SaveChangesAsync();
    }
}