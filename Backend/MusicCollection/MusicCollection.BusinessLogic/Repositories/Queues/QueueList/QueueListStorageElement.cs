using System.ComponentModel.DataAnnotations;
using SqlRepositoryBase.Core.Models;

namespace MusicCollection.BusinessLogic.Repositories.Queues.QueueList;

public class QueueListStorageElement : SqlStorageElement
{
    [Key] public int Position { get; set; }
    public Guid TrackId { get; set; }
}