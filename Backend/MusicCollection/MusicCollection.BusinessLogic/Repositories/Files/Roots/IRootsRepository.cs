﻿using MusicCollection.Api.Dto.FileSystem;

namespace MusicCollection.BusinessLogic.Repositories.Files.Roots;

public interface IRootsRepository : IMusicCollectionRepository
{
    Task<FileSystemRoot> ReadAsync(Guid id);
    Task<FileSystemRoot[]> ReadAllAsync();
    Task CreateAsync(FileSystemRoot root);
    Task DeleteAsync(Guid id);
}