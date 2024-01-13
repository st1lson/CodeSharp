using CodeSharp.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CodeSharp.EntityFramework.Stores;

public abstract class BaseStore<TEntity, TContext>
    where TEntity : class
    where TContext : DbContext
{
    protected DbSet<TEntity> DbSet { get; }
    protected TContext Context { get; }

    protected BaseStore(TContext context)
    {
        Context = context;

        DbSet = Context.Set<TEntity>();
    }

    protected virtual Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Context.SaveChangesAsync(cancellationToken);
    }
}
