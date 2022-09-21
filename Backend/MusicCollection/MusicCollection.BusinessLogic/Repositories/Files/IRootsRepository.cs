using System.Security.AccessControl;
using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.BusinessLogic.Repositories.Database;

namespace MusicCollection.BusinessLogic.Repositories.Files;

public interface IRootsRepository
{
    Task<FileSystemRoot> ReadAsync(Guid id);
    Task<FileSystemRoot> TryReadAsync(Guid id);
    Task CreateAsync(FileSystemRoot root);
    Task<bool> TryDeleteAsync(FileSystemRoot root);
    Task<List<FileSystemRoot>> ReadManyAsync(Guid[] ids);
}