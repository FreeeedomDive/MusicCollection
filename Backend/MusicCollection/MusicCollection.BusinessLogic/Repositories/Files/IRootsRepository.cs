using MusicCollection.BusinessLogic.Repositories.Database;

namespace MusicCollection.BusinessLogic.Repositories.Files;

public interface IRootsRepository
{
    Task<RootStorageElement> Get(Guid id);
    Task CreateRoot(Guid id, string path);
}