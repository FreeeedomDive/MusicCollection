namespace MusicCollection.Api.Dto.Exceptions.Queues;

public class QueueNotFoundException : MusicCollectionApiExceptionBase
{
    public QueueNotFoundException(Guid userId) : base($"Queue for user {userId} doesn't exist") { }
}