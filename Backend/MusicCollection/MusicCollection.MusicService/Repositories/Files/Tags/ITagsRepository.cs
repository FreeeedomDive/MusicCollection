using MusicCollection.Api.Dto.Interfaces;
using MusicCollection.Api.Dto.Music;

namespace MusicCollection.MusicService.Repositories.Files.Tags;

public interface ITagsRepository : IMusicCollectionRepository
{
    public Task<AudioFileTags?> TryReadAsync(Guid id);
    public Task CreateAsync(AudioFileTags audioFileTags);
    public Task DeleteManyAsync(Guid[] ids);
}