using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.BusinessLogic.Extensions;
using MusicCollection.BusinessLogic.Repositories.Files.Music;
using MusicCollection.BusinessLogic.Repositories.Files.Nodes;
using MusicCollection.BusinessLogic.Repositories.Files.Roots;
using MusicCollection.BusinessLogic.Repositories.Files.Tags;
using MusicCollection.BusinessLogic.Utils;
using MusicCollection.Common.TagsService;
using DirectoryNotFoundException = MusicCollection.Api.Dto.Exceptions.Files.DirectoryNotFoundException;

namespace MusicCollection.BusinessLogic.Services.FilesService;

public class FilesService : IFilesService
{
    public FilesService(
        INodesRepository nodesRepository,
        IRootsRepository rootsRepository,
        IMusicFilesRepository musicFilesRepository,
        ITagsRepository tagsRepository,
        ITagsExtractor tagsExtractor
    )
    {
        this.nodesRepository = nodesRepository;
        this.rootsRepository = rootsRepository;
        this.musicFilesRepository = musicFilesRepository;
        this.tagsRepository = tagsRepository;
        this.tagsExtractor = tagsExtractor;
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

    public async Task<FileSystemNode[]> ReadManyNodesAsync(Guid[] ids)
    {
        var nodes = await nodesRepository.ReadManyAsync(ids);
        foreach (var node in nodes)
        {
            await ExtendNodeAsync(node);
        }

        return nodes;
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
        await rootsRepository.CreateAsync(
            new FileSystemRoot
            {
                Id = rootId,
                Name = name,
                Path = path,
            }
        );
        await nodesRepository.CreateAsync(
            new FileSystemNode
            {
                Id = rootId,
                Path = path,
                Type = NodeType.Directory,
            }
        );
        await ProcessDirectoryAsync(rootId, path);

        return rootId;
    }

    public async Task<byte[]> ReadFileContentAsync(Guid id)
    {
        var node = await nodesRepository.ReadAsync(id);
        return await musicFilesRepository.ReadFileAsync(node.Path);
    }

    public async Task<(FileStream, string)> ReadFileContentAsStreamAsync(Guid id)
    {
        var node = await nodesRepository.ReadAsync(id);
        var stream = musicFilesRepository.ReadFileAsStream(node.Path);
        var extension = node.Path.GetFileExtension();
        var extensionsToMimeTypes = MusicFileExtensions.ExtensionToMimeType();
        var mimeType = extensionsToMimeTypes.TryGetValue(extension, out var t) ? t : "";
        return (stream, mimeType);
    }

    public async Task HideNodeAsync(Guid id)
    {
        await nodesRepository.HideNodeAsync(id);
    }

    private async Task<Guid[]> CollectFilesFromDirectoryAsync(Guid directoryId)
    {
        var nodes = await nodesRepository.ReadAllFilesAsync(directoryId, false);
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

        foreach (var file in files)
        {
            var tags = tagsExtractor.TryExtractTags(file.Path);
            if (tags != null)
            {
                tags.Id = file.Id;
                await tagsRepository.CreateAsync(tags);
            }
        }

        await nodesRepository.CreateManyAsync(files.Concat(directories).ToArray());

        foreach (var directory in directories)
        {
            await ProcessDirectoryAsync(directory.Id, directory.Path);
        }
    }

    private static FileSystemNode[] CreateNodes(IEnumerable<string> paths, Guid parentId, NodeType type)
    {
        return paths.Select(
            x => new FileSystemNode
            {
                Id = Guid.NewGuid(),
                ParentId = parentId,
                Path = x,
                Type = type,
            }
        ).ToArray();
    }

    private async Task ExtendNodeAsync(FileSystemNode node)
    {
        if (node.Type == NodeType.Directory)
        {
            var innerData = await nodesRepository.ReadAllFilesAsync(node.Id, false);
            node.DirectoryData = new DirectoryData
            {
                Directories = innerData.Count(x => x.Type == NodeType.Directory),
                Files = innerData.Count(x => x.Type == NodeType.File),
            };
        }
        else
        {
            node.Tags = await tagsRepository.TryReadAsync(node.Id);
        }
    }

    private readonly IMusicFilesRepository musicFilesRepository;

    private readonly INodesRepository nodesRepository;
    private readonly IRootsRepository rootsRepository;
    private readonly ITagsExtractor tagsExtractor;
    private readonly ITagsRepository tagsRepository;
}