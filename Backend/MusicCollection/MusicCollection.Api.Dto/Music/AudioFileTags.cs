namespace MusicCollection.Api.Dto.Music;

public class AudioFileTags
{
    public string? Artist { get; set; }
    public string? Album { get; set; }
    public string? TrackName { get; set; } // in ms
    public string? Duration { get; set; }  
    public string? Format { get; set; }  // mp3 or flac
    public string? SampleFrequency { get; set; } // Hz
    public string? BitRate { get; set; }  // kbps
    public string? BitDepth { get; set; } // bit
}