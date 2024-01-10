using CodeSharp.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CodeSharp.EntityFramework.Stores;

public abstract class BaseStore<TEntity> where TEntity : class
{
    protected DbSet<TEntity> DbSet { get; }
    protected CodeSharpDbContext Context { get; }

    protected BaseStore(CodeSharpDbContext context)
    {
        Context = context;

        DbSet = Context.Set<TEntity>();
    }

    protected virtual Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Context.SaveChangesAsync(cancellationToken);
    }
}
