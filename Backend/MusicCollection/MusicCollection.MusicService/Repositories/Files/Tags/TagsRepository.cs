using MusicCollection.Api.Dto.Music;
using SqlRepositoryBase.Core.Repository;

namespace MusicCollection.MusicService.Repositories.Files.Tags;

public class TagsRepository : ITagsRepository
{
    public TagsRepository(ISqlRepository<AudioFileTagsStorageElement> sqlRepository)
    {
        this.sqlRepository = sqlRepository;
    }

    public async Task<AudioFileTags?> TryReadAsync(Guid id)
    {
        var result = await sqlRepository.TryReadAsync(id);

        return ToModel(result);
    }

    public async Task CreateAsync(AudioFileTags audioFileTags)
    {
        await sqlRepository.CreateAsync(ToStorageElement(audioFileTags));
    }

    public async Task DeleteManyAsync(Guid[] ids)
    {
        await sqlRepository.DeleteAsync(ids);
    }

    private static AudioFileTags? ToModel(AudioFileTagsStorageElement? storageElement)
    {
        return storageElement == null
            ? null
            : new AudioFileTags
            {
                Id = storageElement.Id,
                Artist = storageElement.Artist,
                Album = storageElement.Album,
                TrackName = storageElement.TrackName,
                Duration = storageElement.Duration,
                Format = storageElement.Format,
                SampleFrequency = storageElement.SampleFrequency,
                BitDepth = storageElement.BitDepth,
                BitRate = storageElement.BitRate
            };
    }

    private static AudioFileTagsStorageElement ToStorageElement(AudioFileTags model)
    {
        return new AudioFileTagsStorageElement
        {
            Id = model.Id,
            Artist = model.Artist,
            Album = model.Album,
            TrackName = model.TrackName,
            Duration = model.Duration,
            Format = model.Format,
            SampleFrequency = model.SampleFrequency,
            BitDepth = model.BitDepth,
            BitRate = model.BitRate
        };
    }

    private readonly ISqlRepository<AudioFileTagsStorageElement> sqlRepository;
}
