using SqlRepositoryBase.Core.Models;

namespace MusicCollection.MusicService.Repositories.Queues.QueueContext;

public class QueueContextStorageElement : SqlStorageElement
{
    public Guid ContextId { get; set; }
}