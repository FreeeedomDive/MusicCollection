using MusicCollection.Api.Dto.Music;

namespace MusicCollection.Common.TagsService;

public interface ITagsExtractor
{
    public AudioFileTags TryExtractTags(string path);
}