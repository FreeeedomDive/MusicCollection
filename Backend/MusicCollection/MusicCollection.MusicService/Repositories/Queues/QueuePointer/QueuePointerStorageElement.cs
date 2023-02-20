using SqlRepositoryBase.Core.Models;

namespace MusicCollection.MusicService.Repositories.Queues.QueuePointer;

public class QueuePointerStorageElement : SqlStorageElement
{
    public int Current { get; set; }
}