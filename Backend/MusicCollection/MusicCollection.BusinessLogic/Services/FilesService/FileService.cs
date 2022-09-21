using System.Net;
using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.BusinessLogic.Repositories.Files;

namespace MusicCollection.BusinessLogic.Services.FilesService;

public class FileService : IFileService
{
    private readonly NodesRepository nodesRepository;
    private readonly RootsRepository rootsRepository;

    public FileService(NodesRepository nodesRepository, RootsRepository rootsRepository)
    {
        this.nodesRepository = nodesRepository;
        this.rootsRepository = rootsRepository;
    }

    public async Task<List<FileSystemNode>> ReadAllFiles(Guid parentId)
    {
        return await nodesRepository.ReadAllFilesAsync(parentId);
    }

    public async Task<FileSystemNode> ReadNodeAsync(Guid id)
    {
        return await nodesRepository.ReadAsync(id);
    }
    
    public async Task<FileSystemNode?> TryReadNodeAsync(Guid id)
    {
        return await nodesRepository.TryReadAsync(id);
    }

    public async Task UpdateNodeAsync(FileSystemNode node)
    {
        await nodesRepository.UpdateAsync(node);
    }
    
    public async Task<List<FileSystemNode>> ReadManyNodesAsync(Guid[] ids)
    {
        return await nodesRepository.ReadManyAsync(ids);
    }

    public async Task TryDeleteNodeAsync(FileSystemNode node)
    {
        await nodesRepository.TryDeleteAsync(node);
    }

    public async Task CreateOrUpdateNodeAsync(FileSystemNode node)
    {
        await nodesRepository.CreateOrUpdateAsync(node);
    }

    public async Task<FileSystemRoot> ReadRootAsync(Guid id)
    {
        return await rootsRepository.ReadAsync(id);
    }

    public async Task<FileSystemRoot> TryReadRootAsync(Guid id)
    {
        return await rootsRepository.TryReadAsync(id);
    }

    public async Task CreateRootAsync(FileSystemRoot root)
    {
        await rootsRepository.CreateAsync(root);
    }

    public async Task<bool> TryDeleteAsync(FileSystemRoot root)
    {
        return await rootsRepository.TryDeleteAsync(root);
    }

    public async Task<List<FileSystemRoot>> ReadManyAsync(Guid[] ids)
    {
        return await rootsRepository.ReadManyAsync(ids);
    }

}