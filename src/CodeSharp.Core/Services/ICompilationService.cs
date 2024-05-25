using CodeSharp.Core.Contracts;
using CodeSharp.Core.Executors.Models.Compilation;

namespace CodeSharp.Core.Services;

public interface ICompilationService<TCompilationLog, TKey> where TCompilationLog : ICompilationLog<TKey>
{
    Task<IList<TCompilationLog>> GetCompilationLogsAsync(CancellationToken cancellationToken = default);
    Task<TCompilationLog?> GetCompilationLogAsync(TKey id, CancellationToken cancellationToken = default);
    Task<TCompilationLog> AddCompilationLogAsync(TCompilationLog compilationLog, CancellationToken cancellationToken = default);
    Task<TCompilationLog> CompileAsync(string code, CompilationOptions? options = default, CancellationToken cancellationToken = default);
    Task<TCompilationLog?> RemoveCompilationLogAsync(TKey id, CancellationToken cancellationToken = default);
}
