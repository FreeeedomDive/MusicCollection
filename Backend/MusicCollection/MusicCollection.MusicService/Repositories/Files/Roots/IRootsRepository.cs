using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.Api.Dto.Interfaces;

namespace MusicCollection.MusicService.Repositories.Files.Roots;

public interface IRootsRepository : IMusicCollectionRepository
{
    Task<FileSystemRoot> ReadAsync(Guid id);
    Task<FileSystemRoot[]> ReadAllAsync();
    Task CreateAsync(FileSystemRoot root);
    Task DeleteAsync(Guid id);
}