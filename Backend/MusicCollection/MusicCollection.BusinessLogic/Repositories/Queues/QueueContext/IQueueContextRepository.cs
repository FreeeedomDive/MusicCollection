namespace MusicCollection.BusinessLogic.Repositories.Queues.QueueContext;

public interface IQueueContextRepository : IMusicCollectionRepository
{
    Task CreateOrUpdateAsync(Guid userId, Guid contextId);
    Task<Guid?> TryReadAsync(Guid userId);
    Task DeleteAsync(Guid userId);
}