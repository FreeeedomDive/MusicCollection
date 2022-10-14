using System.ComponentModel.DataAnnotations;

namespace DatabaseCore.Models;

public class SqlStorageElement
{
    [Key]
    public Guid Id { get; set; }
}