using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SqlRepositoryBase.Core.Models;

namespace MusicCollection.BusinessLogic.Repositories.Queues.QueueList;

[PrimaryKey("Id", "Position")]
public class QueueListStorageElement : SqlStorageElement
{
    [Key] public int Position { get; set; }
    public Guid TrackId { get; set; }
}