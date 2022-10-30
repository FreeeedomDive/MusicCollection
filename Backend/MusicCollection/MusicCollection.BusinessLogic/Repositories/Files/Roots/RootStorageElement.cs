using DatabaseCore.Models;

namespace MusicCollection.BusinessLogic.Repositories.Files.Roots;

public class RootStorageElement : SqlStorageElement
{
    public string Name { get; set; }
    public string Path { get; set; }
}