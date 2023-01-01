using SqlRepositoryBase.Core.Models;

namespace MusicCollection.BusinessLogic.Repositories.Files.Tags;

public class AudioFileTagsStorageElement : SqlStorageElement
{
    public string? Artist { get; set; }
    public string? Album { get; set; }
    public string? TrackName { get; set; }
    public string? Duration { get; set; }
    public string? Format { get; set; }
    public int? SampleFrequency { get; set; }
    public int? BitRate { get; set; }
    public int? BitDepth { get; set; }
}