using System.Linq.Expressions;
using DatabaseCore.Models;

namespace DatabaseCore.Repository;

public interface ISqlRepository<TStorageElement> where TStorageElement : SqlStorageElement
{
    Task<TStorageElement[]> ReadAllAsync();
    Task<TStorageElement> ReadAsync(Guid id);
    Task<TStorageElement?> TryReadAsync(Guid id);
    Task<TStorageElement[]> FindAsync(Expression<Func<TStorageElement, bool>> predicate);
    IQueryable<TStorageElement> BuildCustomQuery();
    Task CreateAsync(TStorageElement storageElement);
    Task CreateManyAsync(IEnumerable<TStorageElement> storageElements);
    Task DeleteAsync(Guid id);
}