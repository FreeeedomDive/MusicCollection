using System.ComponentModel.DataAnnotations;

namespace MusicCollection.BusinessLogic.Repositories.Files;

public class NodeStorageElement
{
    [Key]
    public Guid Id { get; set; }
    public Guid ParentId { get; set; }
    public NodeType Type { get; set; }
}