using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.BusinessLogic.Extensions;
using MusicCollection.BusinessLogic.Repositories.Files.Nodes;
using MusicCollection.BusinessLogic.Repositories.Files.Roots;
using MusicCollection.Common.Loggers;
using MusicCollection.Common.TagsService;
using DirectoryNotFoundException = MusicCollection.Api.Dto.Exceptions.DirectoryNotFoundException;

namespace MusicCollection.BusinessLogic.Services.FilesService;

public class FilesService : IFilesService
{
    public FilesService(
        INodesRepository nodesRepository,
        IRootsRepository rootsRepository,
        ITagsExtractor tagsExtractor,
        ILogger logger
    )
    {
        this.nodesRepository = nodesRepository;
        this.rootsRepository = rootsRepository;
        this.tagsExtractor = tagsExtractor;
        this.logger = logger;
    }

    public async Task<FileSystemNode[]> ReadDirectoryAsync(Guid directoryId, int skip = 0, int take = 50)
    {
        var nodes = await nodesRepository.ReadAllFilesAsync(directoryId, true, skip, take);
        foreach (var node in nodes)
        {
            await ExtendNodeAsync(node);
        }

        return nodes;
    }

    public async Task<Guid[]> ReadAllFilesFromDirectoryAsync(Guid directoryId)
    {
        return await CollectFilesFromDirectoryAsync(directoryId);
    }

    public async Task<FileSystemRoot[]> ReadAllRoots()
    {
        return await rootsRepository.ReadAllAsync();
    }

    public async Task<FileSystemNode> ReadNodeAsync(Guid id)
    {
        var node = await nodesRepository.ReadAsync(id);
        await ExtendNodeAsync(node);
        return node;
    }

    public async Task<FileSystemNode?> TryReadNodeAsync(Guid id)
    {
        var node = await nodesRepository.TryReadAsync(id);
        if (node == null)
        {
            return null;
        }
        await ExtendNodeAsync(node);
        return node;
    }

    public async Task<FileSystemRoot> ReadRootAsync(Guid id)
    {
        return await rootsRepository.ReadAsync(id);
    }

    public async Task<Guid> CreateRootWithIndexAsync(string name, string path)
    {
        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException(path);
        }

        var rootId = Guid.NewGuid();
        await rootsRepository.CreateAsync(new FileSystemRoot
        {
            Id = rootId,
            Name = name,
            Path = path
        });
        await nodesRepository.CreateAsync(new FileSystemNode
        {
            Id = rootId,
            Path = path,
            Type = NodeType.Directory
        });
        await ProcessDirectoryAsync(rootId, path);

        return rootId;
    }

    private async Task<Guid[]> CollectFilesFromDirectoryAsync(Guid directoryId)
    {
        var nodes = await nodesRepository.ReadAllFilesAsync(directoryId);
        var files = nodes.Where(x => x.Type == NodeType.File).Select(x => x.Id);
        var directories = nodes.Where(x => x.Type == NodeType.Directory);

        foreach (var directory in directories)
        {
            var filesFromSubdirectory = await CollectFilesFromDirectoryAsync(directory.Id);
            files = files.Concat(filesFromSubdirectory);
        }

        return files.ToArray();
    }

    private async Task ProcessDirectoryAsync(Guid parentId, string path)
    {
        var directories = CreateNodes(Directory.GetDirectories(path), parentId, NodeType.Directory);
        var files = CreateNodes(Directory.GetFiles(path).Where(x => x.IsSupportedExtension()), parentId, NodeType.File);

        logger.Info($"Directories in {path}\n{string.Join("\n", directories.Select(x => x.Path))}");
        logger.Info($"Files in {path}\n{string.Join("\n", files.Select(x => x.Path))}");
        await nodesRepository.CreateManyAsync(files.Concat(directories).ToArray());

        foreach (var directory in directories)
        {
            await ProcessDirectoryAsync(directory.Id, directory.Path);
        }
    }

    private static FileSystemNode[] CreateNodes(IEnumerable<string> paths, Guid parentId, NodeType type)
    {
        return paths.Select(x => new FileSystemNode
        {
            Id = Guid.NewGuid(),
            ParentId = parentId,
            Path = x,
            Type = type
        }).ToArray();
    }

    private async Task ExtendNodeAsync(FileSystemNode node)
    {
        if (node.Type == NodeType.Directory)
        {
            var innerData = await nodesRepository.ReadAllFilesAsync(node.Id, false);
            node.DirectoryData = new DirectoryData
            {
                Directories = innerData.Count(x => x.Type == NodeType.Directory),
                Files = innerData.Count(x => x.Type == NodeType.File)
            };
        }
        else
        {
            node.Tags = tagsExtractor.TryExtractTags(node.Path);
        }
    }

    private readonly INodesRepository nodesRepository;
    private readonly IRootsRepository rootsRepository;
    private readonly ITagsExtractor tagsExtractor;
    private readonly ILogger logger;
}