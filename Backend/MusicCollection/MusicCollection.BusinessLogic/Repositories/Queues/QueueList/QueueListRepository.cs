using Microsoft.EntityFrameworkCore;
using MusicCollection.Api.Dto.Exceptions.Queues;
using SqlRepositoryBase.Core.Repository;

namespace MusicCollection.BusinessLogic.Repositories.Queues.QueueList;

public class QueueListRepository : IQueueListRepository
{
    public QueueListRepository(ISqlRepository<QueueListStorageElement> sqlRepository)
    {
        this.sqlRepository = sqlRepository;
    }

    public async Task CreateAsync(Guid userId, IEnumerable<QueueListElement> elements)
    {
        var storageElements = elements.Select(x => ToStorageElement(userId, x));
        await sqlRepository.CreateAsync(storageElements);
    }

    public async Task<QueueListElement> ReadAsync(Guid userId, int position)
    {
        var result = await sqlRepository.FindAsync(x => x.Id == userId && x.Position == position);
        if (result.Length == 0)
        {
            throw new QueueIndexOutOfRangeException(userId, position);
        }

        // уникальность такого элемента гарантируется уникальностью ключа UserId + Position, поэтому можем вызвать First
        return ToModel(result.First());
    }

    public async Task<QueueListElement[]> ReadManyAsync(Guid userId, int skip = 0, int take = 50)
    {
        var result = await sqlRepository
            .BuildCustomQuery()
            .Where(x => x.Id == userId)
            .OrderBy(x => x.Position)
            .Skip(skip)
            .Take(take)
            .ToArrayAsync();

        return result.Select(ToModel).ToArray();
    }

    public async Task<QueueListElement[]> ReadAllAsync(Guid userId)
    {
        return (await sqlRepository.ReadAllAsync(userId)).Select(ToModel).ToArray();
    }

    public async Task ClearAsync(Guid userId)
    {
        var elements = await sqlRepository.ReadAllAsync(userId);
        await sqlRepository.DeleteAsync(elements);
    }

    private static QueueListElement ToModel(QueueListStorageElement storageElement)
    {
        return new QueueListElement
        {
            Position = storageElement.Position,
            TrackId = storageElement.TrackId
        };
    }

    private static QueueListStorageElement ToStorageElement(Guid userId, QueueListElement queueListElement)
    {
        return new QueueListStorageElement
        {
            Id = userId,
            Position = queueListElement.Position,
            TrackId = queueListElement.TrackId
        };
    }

    private readonly ISqlRepository<QueueListStorageElement> sqlRepository;
}