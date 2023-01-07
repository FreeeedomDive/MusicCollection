namespace MusicCollection.Api.Dto.Exceptions;

public class QueueNotFoundException : Exception
{
    public QueueNotFoundException(Guid userId) : base($"Queue for user {userId} doesn't exist")
    {
    }
}