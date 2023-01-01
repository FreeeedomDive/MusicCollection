using MusicCollection.Api.Dto.Queues;

namespace MusicCollection.BusinessLogic.Repositories.Queues.QueueList;

public interface IQueueListRepository : IMusicCollectionRepository
{
    /// <summary>
    ///     Перед созданием новой очереди не забудь очистить существующую
    /// </summary>
    Task CreateAsync(Guid userId, IEnumerable<QueueListElement> elements);
    Task<QueueListElement> ReadAsync(Guid userId, int position);
    Task<QueueListElement[]> ReadAllAsync(Guid userId);
    Task<bool> IsQueueExist(Guid userId);
    Task ClearAsync(Guid userId);
}