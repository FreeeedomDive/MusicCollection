using MusicCollection.Api.Dto.Exceptions.Api;

namespace MusicCollection.Api.Dto.Exceptions.Queues;

public class QueueIndexOutOfRangeException : MusicCollectionApiConflictException
{
    public QueueIndexOutOfRangeException(Guid userId, int position)
        : base($"Track in position {position} doesn't exist in queue of user {userId}")
    {
    }
}