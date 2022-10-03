using MusicCollection.Api.Dto.Music;
using TagExtractor.Extensions;

namespace TagExtractor;

public class TagsExtractor : ITagsExtractor
{
    public AudioFileTags? ExtractTags(string path)
    {
        try
        {
            var tagFile = TagLib.File.Create(path);
            var duration = tagFile.Properties.Duration;
            return new AudioFileTags
            {
                TrackName = tagFile.Tag.Title,
                Artist = tagFile.Tag.FirstPerformer,
                Album = tagFile.Tag.Album,
                Duration = $"{duration.Minutes}:{duration.Seconds.NormalizeToLength(2)}",
                Format = path.GetFileExtension(),
                BitRate = tagFile.Properties.AudioBitrate.ToString(),
                BitDepth = tagFile.Properties.BitsPerSample.ToString(),
                SampleFrequency = tagFile.Properties.AudioSampleRate.ToString()
            };
        }
        catch
        {
            return null;
        }
    }
}