using MusicCollection.Api.Dto.Interfaces;

namespace MusicCollection.MusicService.Repositories.Queues.QueuePointer;

public interface IQueuePointerRepository : IMusicCollectionRepository
{
    Task CreateOrUpdateAsync(Guid userId, int position);
    Task<int?> TryReadAsync(Guid userId);
    Task DeleteAsync(Guid userId);
}