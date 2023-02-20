using SqlRepositoryBase.Core.Models;

namespace MusicCollection.MusicService.Repositories.Files.Roots;

public class RootStorageElement : SqlStorageElement
{
    public string Name { get; set; }
    public string Path { get; set; }
}