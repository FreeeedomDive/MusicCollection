using MusicCollection.Api.Dto.Music;

namespace MusicCollection.BusinessLogic.Repositories.Files.Tags;

public interface ITagsRepository : IMusicCollectionRepository
{
    public Task<AudioFileTags?> TryReadAsync(Guid id);
    public Task CreateAsync(AudioFileTags audioFileTags);
}