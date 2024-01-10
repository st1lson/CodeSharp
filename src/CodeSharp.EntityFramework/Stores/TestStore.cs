using CodeSharp.Core.Contracts;
using CodeSharp.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CodeSharp.EntityFramework.Stores;

public class TestStore<TItem> : TestStore<TItem, Guid>
    where TItem : class, ITest<Guid>
{
    public TestStore(CodeSharpDbContext context) : base(context)
    {
    }
}

public class TestStore<TItem, TKey> : BaseStore<TItem>, ITestStore<TItem, TKey> where TItem : class, ITest<TKey>
{
    public TestStore(CodeSharpDbContext context) : base(context)
    {
    }

    public Task CreateAsync(TItem item, CancellationToken cancellationToken = default)
    {
        DbSet.Add(item);
        return SaveChangesAsync(cancellationToken);
    }

    public Task UpdateAsync(TItem item, CancellationToken cancellationToken = default)
    {
        Context.Entry(item).State = EntityState.Modified;
        return SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TKey key, CancellationToken cancellationToken = default)
    {
        var item = await GetByIdAsync(key, cancellationToken);
        if (item is null)
        {
            return;
        }

        DbSet.Remove(item);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task<TItem?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IList<TItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.ToListAsync(cancellationToken);
    }
}