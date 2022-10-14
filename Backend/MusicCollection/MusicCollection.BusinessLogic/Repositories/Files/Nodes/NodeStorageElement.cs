using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using MusicCollection.Api.Dto.FileSystem;

namespace MusicCollection.BusinessLogic.Repositories.Files.Nodes;

[Index(nameof(Id), nameof(Path))]
public class NodeStorageElement
{
    [Key]
    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
    public NodeType Type { get; set; }
    public string Path { get; set; }
}