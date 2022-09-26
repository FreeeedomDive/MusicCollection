using System.Net;
using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.BusinessLogic.Repositories.Files;

namespace MusicCollection.BusinessLogic.Services.FilesService;

public class FilesService : IFilesService
{
    private readonly INodesRepository nodesRepository;
    private readonly IRootsRepository rootsRepository;

    public FilesService(INodesRepository nodesRepository, IRootsRepository rootsRepository)
    {
        this.nodesRepository = nodesRepository;
        this.rootsRepository = rootsRepository;
    }

    public async Task<FileSystemNode[]> ReadAllFiles(Guid parentId)
    {
        return await nodesRepository.ReadAllFilesAsync(parentId);
    }

    public async Task<FileSystemRoot[]> ReadAllRoots()
    {
        return await rootsRepository.ReadAllAsync();
    }

    public async Task<FileSystemNode> ReadNodeAsync(Guid id)
    {
        return await nodesRepository.ReadAsync(id);
    }
    
    public async Task<FileSystemNode?> TryReadNodeAsync(Guid id)
    {
        return await nodesRepository.TryReadAsync(id);
    }

    public async Task<FileSystemRoot> ReadRootAsync(Guid id)
    {
        return await rootsRepository.ReadAsync(id);
    }

    public async Task CreateRootAsync(FileSystemRoot root)
    {
        await rootsRepository.CreateAsync(root);
    }
}