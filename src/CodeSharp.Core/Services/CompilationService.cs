﻿using CodeSharp.Core.Contracts;
using CodeSharp.Core.Executors;
using CodeSharp.Core.Executors.Models.Compilation;

namespace CodeSharp.Core.Services;

public class CompilationService<TCompilationLog> : CompilationService<TCompilationLog, Guid> where TCompilationLog : ICompilationLog<Guid>
{
    public CompilationService(ICompileExecutor<TCompilationLog> compileExecutor, ICompilationLogStore<TCompilationLog, Guid> compilationLogStore)
        : base(compileExecutor, compilationLogStore)
    {
    }
}

public class CompilationService<TCompilationLog, TKey> : ICompilationService<TCompilationLog, TKey> where TCompilationLog : ICompilationLog<TKey>
{
    private readonly ICompilationLogStore<TCompilationLog, TKey> _compilationLogStore;
    private readonly ICompileExecutor<TCompilationLog> _compileExecutor;

    public CompilationService(ICompileExecutor<TCompilationLog> compileExecutor, ICompilationLogStore<TCompilationLog, TKey> compilationLogStore)
    {
        _compileExecutor = compileExecutor;
        _compilationLogStore = compilationLogStore;
    }

    public Task<TCompilationLog> AddCompilationLogAsync(TCompilationLog compilationLog, CancellationToken cancellationToken = default)
    {
        return _compilationLogStore.CreateAsync(compilationLog, cancellationToken);
    }

    public async Task<TCompilationLog> CompileAsync(string code, CompilationOptions? options = default, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(code))
        {
            throw new ArgumentNullException(nameof(code));
        }

        var compilationLog = await _compileExecutor.CompileAsync(code, options, cancellationToken);
        await _compilationLogStore.CreateAsync(compilationLog, cancellationToken);

        return compilationLog;
    }

    public Task<TCompilationLog?> GetCompilationLogAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return _compilationLogStore.GetByIdAsync(id, cancellationToken);
    }

    public Task<IList<TCompilationLog>> GetCompilationLogsAsync(CancellationToken cancellationToken = default)
    {
        return _compilationLogStore.GetAllAsync(cancellationToken);
    }

    public Task<TCompilationLog?> RemoveCompilationLogAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return _compilationLogStore.RemoveAsync(id, cancellationToken);
    }
}
