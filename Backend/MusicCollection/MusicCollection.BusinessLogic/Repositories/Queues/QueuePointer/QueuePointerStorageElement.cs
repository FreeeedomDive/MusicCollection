using SqlRepositoryBase.Core.Models;

namespace MusicCollection.BusinessLogic.Repositories.Queues.QueuePointer;

public class QueuePointerStorageElement : SqlStorageElement
{
    public int Current { get; set; }
}