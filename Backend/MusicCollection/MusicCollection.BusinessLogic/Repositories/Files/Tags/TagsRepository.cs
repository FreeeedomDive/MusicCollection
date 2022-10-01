using Microsoft.EntityFrameworkCore;
using MusicCollection.Api.Dto.Music;
using MusicCollection.BusinessLogic.Repositories.Database;

namespace MusicCollection.BusinessLogic.Repositories.Files.Tags;

public class TagsRepository : ITagsRepository
{
    public TagsRepository(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }

    public async Task<AudioFileTags?> TryReadAsync(Guid id)
    {
        var result = await databaseContext.AudioFileTagsStorage
            .FirstOrDefaultAsync(x => x.Id == id);

        return ToModel(result);
    }

    public async Task CreateAsync(AudioFileTags audioFileTags)
    {
        await databaseContext.AudioFileTagsStorage.AddAsync(ToStorageElement(audioFileTags));
        await databaseContext.SaveChangesAsync();
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

    private readonly DatabaseContext databaseContext;
}