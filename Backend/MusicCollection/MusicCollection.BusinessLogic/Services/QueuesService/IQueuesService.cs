using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.Api.Dto.Queues;

namespace MusicCollection.BusinessLogic.Services.QueuesService;

public interface IQueuesService : IMusicCollectionLogicService
{
    Task CreateQueueAsync(Guid userId, Guid contextId);
    Task<FileSystemNode> GetCurrentContextAsync(Guid userId);
    Task ClearQueueAsync(Guid userId);

    Task<QueueTrack?> GetCurrentAsync(Guid userId);
    Task<QueueTrack[]> GetQueue(Guid userId);

    Task<QueueTrack> MovePreviousAsync(Guid userId);
    Task<QueueTrack> MoveNextAsync(Guid userId);
    Task<QueueTrack> MoveToPositionAsync(Guid userId, int nextPosition);
}