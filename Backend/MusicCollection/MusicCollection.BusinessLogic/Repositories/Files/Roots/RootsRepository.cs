using MusicCollection.Api.Dto.Exceptions.Files;
using MusicCollection.Api.Dto.FileSystem;
using SqlRepositoryBase.Core.Exceptions;
using SqlRepositoryBase.Core.Repository;

namespace MusicCollection.BusinessLogic.Repositories.Files.Roots;

public class RootsRepository : IRootsRepository
{
    public RootsRepository(ISqlRepository<RootStorageElement> sqlRepository)
    {
        this.sqlRepository = sqlRepository;
    }

    public async Task<FileSystemRoot> ReadAsync(Guid id)
    {
        try
        {
            var result = await sqlRepository.ReadAsync(id);
            return ToModel(result);
        }
        catch (SqlEntityNotFoundException)
        {
            throw new RootNotFoundException(id);
        }
    }

    public async Task<FileSystemRoot[]> ReadAllAsync()
    {
        var result = await sqlRepository.ReadAllAsync();
        return result.Select(ToModel).OrderBy(x => x.Name).ToArray();
    }

    public async Task CreateAsync(FileSystemRoot root)
    {
        await sqlRepository.CreateAsync(ToStorageElement(root));
    }

    public async Task DeleteAsync(Guid id)
    {
        await sqlRepository.DeleteAsync(id);
    }

    private static FileSystemRoot ToModel(RootStorageElement root)
    {
        return new FileSystemRoot
        {
            Id = root.Id,
            Name = root.Name,
            Path = root.Path
        };
    }

    private static RootStorageElement ToStorageElement(FileSystemRoot root)
    {
        return new RootStorageElement
        {
            Id = root.Id,
            Name = root.Name,
            Path = root.Path
        };
    }

    private readonly ISqlRepository<RootStorageElement> sqlRepository;
}