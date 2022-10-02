using System.ComponentModel.DataAnnotations;
using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.Api.Dto.Music;

namespace MusicCollection.BusinessLogic.Repositories.Files.Nodes;

public class NodeStorageElement
{
    [Key]
    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
    public NodeType Type { get; set; }
    public string Path { get; set; }
}