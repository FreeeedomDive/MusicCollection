using DatabaseCore.Models;

namespace MusicCollection.BusinessLogic.Repositories.Files.Roots;

public class RootStorageElement : SqlStorageElement
{
    public string Path { get; set; }
}