using System.Linq.Expressions;

namespace DatabaseCore.Models;

public class SqlFilterOptions<TStorageElement, TKey>
    where TStorageElement : SqlStorageElement
{
    public Expression<Func<TStorageElement, bool>> FilterRule { get; set; }
    public SqlOrderOptions<TStorageElement, TKey>[]? OrderOptions { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}