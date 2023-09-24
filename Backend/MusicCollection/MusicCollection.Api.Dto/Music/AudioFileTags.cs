namespace MusicCollection.Api.Dto.Music;

public class AudioFileTags
{
    public Guid Id { get; set; }
    public string? Artist { get; set; }
    public string? Album { get; set; }
    public string? TrackName { get; set; }
    public string? Duration { get; set; }
    public string? Format { get; set; }
    public int? SampleFrequency { get; set; }
    public int? BitRate { get; set; }
    public int? BitDepth { get; set; }
}