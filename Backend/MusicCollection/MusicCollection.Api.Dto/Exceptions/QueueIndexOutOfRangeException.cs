namespace MusicCollection.Api.Dto.Exceptions;

public class QueueIndexOutOfRangeException : Exception
{
    public QueueIndexOutOfRangeException(Guid userId, int position)
        : base($"Track in position {position} doesn't exist in queue of user {userId}")
    {
    }
}