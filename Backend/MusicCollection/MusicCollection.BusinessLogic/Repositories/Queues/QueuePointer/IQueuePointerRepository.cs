namespace MusicCollection.BusinessLogic.Repositories.Queues.QueuePointer;

public interface IQueuePointerRepository : IMusicCollectionRepository
{
    Task CreateOrUpdateAsync(Guid userId, int position);
    Task<int?> TryReadAsync(Guid userId);
}