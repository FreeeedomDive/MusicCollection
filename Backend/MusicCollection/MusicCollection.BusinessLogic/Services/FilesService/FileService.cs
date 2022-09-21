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

    public async Task CreateNode(NodeStorageElement node)
    {
        await nodesRepository.CreateNode(node);
    }

    public async Task<List<NodeStorageElement>> GetFilesInDirectory(Guid parentId)
    {
        return await nodesRepository.GetFilesInDirectory(parentId);
    }

    public async Task<NodeStorageElement> GetFile(Guid id)
    {
        
    }

}