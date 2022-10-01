using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.BusinessLogic.Extensions;
using MusicCollection.BusinessLogic.Repositories.Files.Nodes;
using MusicCollection.BusinessLogic.Repositories.Files.Roots;
using MusicCollection.BusinessLogic.Repositories.Files.Tags;

namespace MusicCollection.BusinessLogic.Services.FilesService;

public class FilesService : IFilesService
{
    private readonly INodesRepository nodesRepository;
    private readonly IRootsRepository rootsRepository;
    private readonly ITagsRepository tagsRepository;

    public FilesService(
        INodesRepository nodesRepository,
        IRootsRepository rootsRepository,
        ITagsRepository tagsRepository
    )
    {
        this.nodesRepository = nodesRepository;
        this.rootsRepository = rootsRepository;
        this.tagsRepository = tagsRepository;
    }

    public async Task<FileSystemNode[]> ReadAllFiles(Guid parentId)
    {
        var nodes = await nodesRepository.ReadAllFilesAsync(parentId);
        var extendTasks = nodes.Select(x => x.ExtendAsync(ExtendFileWithTagsAsync));
        return await Task.WhenAll(extendTasks);
    }

    public async Task<FileSystemRoot[]> ReadAllRoots()
    {
        return await rootsRepository.ReadAllAsync();
    }

    public async Task<FileSystemNode> ReadNodeAsync(Guid id)
    {
        var node = await nodesRepository.ReadAsync(id);
        return await node.ExtendAsync(ExtendFileWithTagsAsync);
    }

    public async Task<FileSystemNode?> TryReadNodeAsync(Guid id)
    {
        var node = await nodesRepository.TryReadAsync(id);
        return node == null ? null : await node.ExtendAsync(ExtendFileWithTagsAsync);
    }

    public async Task<FileSystemRoot> ReadRootAsync(Guid id)
    {
        return await rootsRepository.ReadAsync(id);
    }

    public async Task CreateRootAsync(FileSystemRoot root)
    {
        await rootsRepository.CreateAsync(root);
    }

    private async Task ExtendFileWithTagsAsync(FileSystemNode fileSystemNode)
    {
        if (fileSystemNode == null)
        {
            return;
        }
        if (fileSystemNode.Type == NodeType.Directory)
        {
            return;
        }

        fileSystemNode.Tags = await tagsRepository.TryReadAsync(fileSystemNode.Id);
    }
}