namespace MusicCollection.Api.Dto.FileSystem;

public class FileSystemNode
{
    public Guid Id { get; }
    public Guid ParentId { get; }
    public NodeType Type { get; }
}