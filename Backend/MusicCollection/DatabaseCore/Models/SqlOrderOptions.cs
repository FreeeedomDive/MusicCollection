using System.Linq.Expressions;

namespace DatabaseCore.Models;

public class SqlOrderOptions<TStorageElement, TKey>
    where TStorageElement : SqlStorageElement
{
    public Func<TStorageElement, TKey> OrderRule { get; set; }
    public bool OrderByDescending { get; set; } = false;
}