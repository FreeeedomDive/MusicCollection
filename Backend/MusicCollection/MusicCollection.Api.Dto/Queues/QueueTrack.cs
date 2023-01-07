using MusicCollection.Api.Dto.FileSystem;

namespace MusicCollection.Api.Dto.Queues;

public class QueueTrack
{
    public int Position { get; set; }
    public FileSystemNode Track { get; set; }
}