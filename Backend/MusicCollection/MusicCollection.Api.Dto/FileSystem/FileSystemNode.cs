using MusicCollection.Api.Dto.Music;

namespace MusicCollection.Api.Dto.FileSystem;

public class FileSystemNode
{
    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
    public NodeType Type { get; set; }
    public AudioFileTags? Tags { get; set; }
}