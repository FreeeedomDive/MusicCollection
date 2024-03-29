using MusicCollection.Api.Dto.Music;
using MusicCollection.Common.TagsService.Extensions;
using File = TagLib.File;

namespace MusicCollection.Common.TagsService;

public class TagsExtractor : ITagsExtractor
{
    public AudioFileTags TryExtractTags(string path)
    {
        try
        {
            var tagFile = File.Create(path);
            var duration = tagFile.Properties.Duration;
            return new AudioFileTags
            {
                TrackName = tagFile.Tag.Title,
                Artist = tagFile.Tag.FirstPerformer,
                Album = tagFile.Tag.Album,
                Duration = $"{duration.Minutes}:{duration.Seconds.NormalizeToLength(2)}",
                Format = path.GetFileExtension(),
                BitRate = tagFile.Properties.AudioBitrate,
                BitDepth = tagFile.Properties.BitsPerSample,
                SampleFrequency = tagFile.Properties.AudioSampleRate,
            };
        }
        catch
        {
            return new AudioFileTags
            {
                Format = path.GetFileExtension(),
            };
        }
    }
}