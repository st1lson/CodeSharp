using CodeSharp.Core.Contracts;
using CodeSharp.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CodeSharp.EntityFramework.Stores;

public class CompilationLogStore<TCompilationLog> : CompilationLogStore<TCompilationLog, Guid, CodeSharpDbContext>
    where TCompilationLog : class, ICompilationLog<Guid>
{
    public CompilationLogStore(CodeSharpDbContext context) : base(context)
    {
    }
}

public class CompilationLogStore<TCompilationLog, TKey, TContext> :
    BaseStore<TCompilationLog, TContext>,
    ICompilationLogStore<TCompilationLog, TKey>
    where TCompilationLog : class, ICompilationLog<TKey>
    where TContext : DbContext
{
    public CompilationLogStore(TContext context) : base(context)
    {
    }

    public Task CreateAsync(TCompilationLog item, CancellationToken cancellationToken = default)
    {
        DbSet.Add(item);
        return SaveChangesAsync(cancellationToken);
    }

    public async Task<TCompilationLog?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync(new object[] { id ?? throw new ArgumentNullException(nameof(id)) }, cancellationToken);
    }

    public async Task<IList<TCompilationLog>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.ToListAsync(cancellationToken);
    }

    public async Task RemoveAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var compilationLog = await GetByIdAsync(id, cancellationToken);
        if (compilationLog is null)
        {
            return;
        }

        DbSet.Remove(compilationLog);

        await SaveChangesAsync(cancellationToken);
    }
}