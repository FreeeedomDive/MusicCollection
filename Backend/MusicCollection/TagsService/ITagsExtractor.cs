using MusicCollection.Api.Dto.Music;

namespace TagExtractor;

public interface ITagsExtractor
{
    public AudioFileTags? ExtractTags(string path);
}