using CodeSharp.Core.Contracts;
using CodeSharp.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CodeSharp.EntityFramework.Stores;

public class TestStore<TTest> :
    TestStore<TTest, Guid, CodeSharpDbContext>
    where TTest : class,
    ITest<Guid>
{
    public TestStore(CodeSharpDbContext context) : base(context)
    {
    }
}

public class TestStore<TTest, TKey, TContext> :
    BaseStore<TTest, TContext>,
    ITestStore<TTest, TKey>
    where TTest : class, ITest<TKey>
    where TContext : DbContext
{
    public TestStore(TContext context) : base(context)
    {
    }

    public async Task<IList<TTest>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.ToListAsync(cancellationToken);
    }

    public async Task<TTest?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync(new object[] { id ?? throw new ArgumentNullException(nameof(id)) }, cancellationToken);
    }

    public async Task<TTest> CreateAsync(TTest item, CancellationToken cancellationToken = default)
    {
        DbSet.Add(item);
        await SaveChangesAsync(cancellationToken);

        return item;
    }

    public async Task<TTest> UpdateAsync(TTest item, CancellationToken cancellationToken = default)
    {
        Context.Entry(item).State = EntityState.Modified;
        await SaveChangesAsync(cancellationToken);

        return item;
    }

    public async Task<TTest?> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
    {
        var test = await GetByIdAsync(key, cancellationToken);
        if (test is null)
        {
            return default;
        }

        DbSet.Remove(test);
        await SaveChangesAsync(cancellationToken);

        return test;
    }
}