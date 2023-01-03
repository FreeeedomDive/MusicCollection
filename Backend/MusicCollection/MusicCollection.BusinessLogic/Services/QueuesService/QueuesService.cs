using MusicCollection.Api.Dto.Exceptions;
using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.Api.Dto.Queues;
using MusicCollection.BusinessLogic.Repositories.Queues.QueueContext;
using MusicCollection.BusinessLogic.Repositories.Queues.QueueList;
using MusicCollection.BusinessLogic.Repositories.Queues.QueuePointer;
using MusicCollection.BusinessLogic.Services.FilesService;

namespace MusicCollection.BusinessLogic.Services.QueuesService;

public class QueuesService : IQueuesService
{
    public QueuesService(
        IQueueContextRepository queueContextRepository,
        IQueuePointerRepository queuePointerRepository,
        IQueueListRepository queueListRepository,
        IFilesService filesService
    )
    {
        this.queueContextRepository = queueContextRepository;
        this.queuePointerRepository = queuePointerRepository;
        this.queueListRepository = queueListRepository;
        this.filesService = filesService;
    }

    public async Task CreateQueueAsync(Guid userId, Guid contextId)
    {
        // пока считаем, что contextId - это папка, но в будущем надо будет научиться понимать, что это - папка или плейлист
        // перед созданием новой очереди очищаем текущую
        await ClearQueueAsync(userId);

        await queueContextRepository.CreateOrUpdateAsync(userId, contextId);
        await queuePointerRepository.CreateOrUpdateAsync(userId, 1);

        var allFiles = await filesService.ReadAllFilesFromDirectoryAsync(contextId);
        var queueElements = allFiles.Select((x, i) => new QueueListElement
        {
            Position = i + 1,
            TrackId = x
        });
        await queueListRepository.CreateAsync(userId, queueElements);
    }

    public async Task<FileSystemNode> GetCurrentContextAsync(Guid userId)
    {
        var currentContext = await queueContextRepository.TryReadAsync(userId);
        if (currentContext == null)
        {
            throw new QueueNotFoundException(userId);
        }

        return await filesService.ReadNodeAsync(currentContext.Value);
    }

    public async Task ClearQueueAsync(Guid userId)
    {
        await queueContextRepository.DeleteAsync(userId);
        await queuePointerRepository.DeleteAsync(userId);
        await queueListRepository.ClearAsync(userId);
    }

    public async Task<QueueTrack?> GetCurrentAsync(Guid userId)
    {
        var currentQueuePosition = await queuePointerRepository.TryReadAsync(userId);
        if (currentQueuePosition == null)
        {
            return null;
        }

        var currentTrackId = await queueListRepository.ReadAsync(userId, currentQueuePosition.Value);
        return new QueueTrack
        {
            Position = currentQueuePosition.Value,
            Track = await filesService.ReadNodeAsync(currentTrackId.TrackId)
        };
    }

    public async Task<QueueTrack[]> GetQueue(Guid userId)
    {
        var currentQueuePosition = await queuePointerRepository.TryReadAsync(userId);
        if (currentQueuePosition == null)
        {
            return Array.Empty<QueueTrack>();
        }

        var queue = await queueListRepository.ReadManyAsync(userId, currentQueuePosition.Value);
        var trackIdToPosition = queue.ToDictionary(x => x.TrackId, x => x.Position);
        var nodes = await filesService.ReadManyNodesAsync(queue.Select(x => x.TrackId).ToArray());

        return nodes.Select(x => new QueueTrack
            {
                Position = trackIdToPosition[x.Id],
                Track = x
            })
            .OrderBy(x => x.Position)
            .ToArray();
    }

    public async Task<QueueTrack> MovePreviousAsync(Guid userId)
    {
        return await MoveToPositionAsync(userId, x => x - 1);
    }

    public async Task<QueueTrack> MoveNextAsync(Guid userId)
    {
        return await MoveToPositionAsync(userId, x => x + 1);
    }

    public async Task<QueueTrack> MoveToPositionAsync(Guid userId, int nextPosition)
    {
        return await MoveToPositionAsync(userId, _ => nextPosition);
    }

    private async Task<QueueTrack> MoveToPositionAsync(Guid userId, Func<int, int> modifyCurrentPosition)
    {
        var currentQueuePosition = await queuePointerRepository.TryReadAsync(userId);
        if (currentQueuePosition == null)
        {
            throw new QueueNotFoundException(userId);
        }

        var nextPosition = modifyCurrentPosition(currentQueuePosition.Value);
        var nextTrack = await queueListRepository.ReadAsync(userId, nextPosition);
        await queuePointerRepository.CreateOrUpdateAsync(userId, nextPosition);
        var track = await filesService.ReadNodeAsync(nextTrack.TrackId);

        return new QueueTrack
        {
            Position = nextTrack.Position,
            Track = track
        };
    }

    private readonly IQueueContextRepository queueContextRepository;
    private readonly IQueuePointerRepository queuePointerRepository;
    private readonly IQueueListRepository queueListRepository;
    private readonly IFilesService filesService;
}