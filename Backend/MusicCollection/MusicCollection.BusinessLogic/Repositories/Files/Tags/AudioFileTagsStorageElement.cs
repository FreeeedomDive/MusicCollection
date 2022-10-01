using System.ComponentModel.DataAnnotations;

namespace MusicCollection.BusinessLogic.Repositories.Files.Tags;

public class AudioFileTagsStorageElement
{
    [Key]
    public Guid Id { get; set; }
    public string? Artist { get; set; }
    public string? Album { get; set; }
    public string? TrackName { get; set; }
    public string? Duration { get; set; }  
    public string? Format { get; set; }
    public string? SampleFrequency { get; set; }
    public string? BitRate { get; set; }
    public string? BitDepth { get; set; }
}