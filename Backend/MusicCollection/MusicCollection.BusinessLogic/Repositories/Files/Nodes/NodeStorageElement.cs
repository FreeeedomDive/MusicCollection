using Microsoft.EntityFrameworkCore;
using MusicCollection.Api.Dto.FileSystem;
using SqlRepositoryBase.Core.Models;

namespace MusicCollection.BusinessLogic.Repositories.Files.Nodes;

[Index(nameof(Id), nameof(Path))]
public class NodeStorageElement : SqlStorageElement
{
    public Guid? ParentId { get; set; }
    public NodeType Type { get; set; }
    public string Path { get; set; }
    public bool Hidden { get; set; }
}