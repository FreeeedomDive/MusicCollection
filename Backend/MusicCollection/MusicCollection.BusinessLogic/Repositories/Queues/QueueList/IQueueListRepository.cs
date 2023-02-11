using MusicCollection.Api.Dto.Queues;

namespace MusicCollection.BusinessLogic.Repositories.Queues.QueueList;

public interface IQueueListRepository : IMusicCollectionRepository
{
    /// <summary>
    ///     Перед созданием новой очереди не забудь очистить существующую
    /// </summary>
    Task CreateAsync(Guid userId, IEnumerable<QueueListElement> elements);
    Task<QueueListElement> ReadAsync(Guid userId, int position);
    Task<QueueListElement[]> ReadManyAsync(Guid userId, int skip = 0, int take = 50);
    Task<QueueListElement[]> ReadAllAsync(Guid userId);
    Task ClearAsync(Guid userId);
}