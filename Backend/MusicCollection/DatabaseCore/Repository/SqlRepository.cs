using System.Linq.Expressions;
using DatabaseCore.Exceptions;
using DatabaseCore.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseCore.Repository;

public class SqlRepository<TStorageElement> : ISqlRepository<TStorageElement> where TStorageElement : SqlStorageElement
{
    public SqlRepository(DbContext databaseContext)
    {
        this.databaseContext = databaseContext;
        storage = databaseContext.Set<TStorageElement>();
    }

    public async Task<TStorageElement[]> ReadAllAsync()
    {
        return await storage.ToArrayAsync();
    }

    public async Task<TStorageElement> ReadAsync(Guid id)
    {
        var result = await TryReadAsync(id);
        if (result == null)
        {
            throw new SqlEntityNotFoundException(id);
        }

        return result;
    }

    public async Task<TStorageElement[]> ReadManyAsync(Guid[] ids)
    {
        return await storage.Where(x => ids.Contains(x.Id)).ToArrayAsync();
    }

    public async Task<TStorageElement?> TryReadAsync(Guid id)
    {
        return await storage.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<TStorageElement[]> FindAsync(Expression<Func<TStorageElement, bool>> predicate)
    {
        return await storage.Where(predicate).ToArrayAsync();
    }

    public IQueryable<TStorageElement> BuildCustomQuery()
    {
        return storage.AsQueryable();
    }

    public async Task CreateAsync(TStorageElement storageElement)
    {
        await storage.AddAsync(storageElement);
        await databaseContext.SaveChangesAsync();
    }

    public async Task CreateManyAsync(IEnumerable<TStorageElement> storageElements)
    {
        await storage.AddRangeAsync(storageElements);
        await databaseContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, Action<TStorageElement> updateAction)
    {
        var @object = await storage.FirstAsync(x => x.Id == id);
        updateAction(@object);
        await databaseContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var @object = await TryReadAsync(id);
        if (@object is null)
        {
            return;
        }
        storage.Remove(@object);
        await databaseContext.SaveChangesAsync();
    }

    public async Task DeleteManyAsync(Guid[] ids)
    {
        var objects = await ReadManyAsync(ids);
        storage.RemoveRange(objects);
        await databaseContext.SaveChangesAsync();
    }

    private readonly DbContext databaseContext;
    private readonly DbSet<TStorageElement> storage;
}