using System.ComponentModel.DataAnnotations;
using DatabaseCore.Models;
using Microsoft.EntityFrameworkCore;
using MusicCollection.Api.Dto.FileSystem;

namespace MusicCollection.BusinessLogic.Repositories.Files.Nodes;

[Index(nameof(Id), nameof(Path))]
public class NodeStorageElement : SqlStorageElement
{
    public Guid? ParentId { get; set; }
    public NodeType Type { get; set; }
    public string Path { get; set; }
    public bool Hidden { get; set; }
}