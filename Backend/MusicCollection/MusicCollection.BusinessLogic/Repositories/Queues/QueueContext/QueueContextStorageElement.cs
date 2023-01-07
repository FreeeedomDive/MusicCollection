using SqlRepositoryBase.Core.Models;

namespace MusicCollection.BusinessLogic.Repositories.Queues.QueueContext;

public class QueueContextStorageElement : SqlStorageElement
{
    public Guid ContextId { get; set; }
}