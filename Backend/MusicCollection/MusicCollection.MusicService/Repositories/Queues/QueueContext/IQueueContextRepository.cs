using MusicCollection.Api.Dto.Interfaces;

namespace MusicCollection.MusicService.Repositories.Queues.QueueContext;

public interface IQueueContextRepository : IMusicCollectionRepository
{
    Task CreateOrUpdateAsync(Guid userId, Guid contextId);
    Task<Guid?> TryReadAsync(Guid userId);
    Task DeleteAsync(Guid userId);
}