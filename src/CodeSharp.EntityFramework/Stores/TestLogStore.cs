﻿using CodeSharp.Core.Contracts;
using CodeSharp.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CodeSharp.EntityFramework.Stores;

public class TestLogStore<TTestLog> : TestLogStore<TTestLog, Guid, CodeSharpDbContext>
    where TTestLog : class, ITestLog<Guid>
{
    public TestLogStore(CodeSharpDbContext context) : base(context)
    {
    }
}

public class TestLogStore<TTestLog, TKey, TContext> :
    BaseStore<TTestLog, TContext>,
    ITestLogStore<TTestLog, TKey>
    where TTestLog : class, ITestLog<TKey>
    where TContext : DbContext
{
    public TestLogStore(TContext context) : base(context)
    {
    }

    public async Task<IList<TTestLog>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.ToListAsync(cancellationToken);
    }

    public async Task<TTestLog?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync(new object[] { id ?? throw new ArgumentNullException(nameof(id)) }, cancellationToken);
    }

    public async Task<TTestLog> CreateAsync(TTestLog item, CancellationToken cancellationToken = default)
    {
        DbSet.Add(item);
        await SaveChangesAsync(cancellationToken);

        return item;
    }

    public async Task<TTestLog?> RemoveAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var testLog = await DbSet.FindAsync(new object[] { id ?? throw new ArgumentNullException(nameof(id)) }, cancellationToken);
        if (testLog is null)
        {
            return default;
        }

        DbSet.Remove(testLog);
        await SaveChangesAsync(cancellationToken);

        return testLog;
    }
}