using CodeSharp.Core.Contracts;

namespace CodeSharp.Core.Services;

public interface ICompilationService<TCompilationLog, TKey> where TCompilationLog : ICompilationLog<TKey>
{
    Task<IList<TCompilationLog>> GetCompilationLogsAsync(CancellationToken cancellationToken = default);
    Task<TCompilationLog?> GetCompilationLogAsync(TKey id, CancellationToken cancellationToken = default);
    Task AddCompilationLogAsync(TCompilationLog compilationLog, CancellationToken cancellationToken = default);
    Task<TCompilationLog> CompileAsync(string code, bool run = false, CancellationToken cancellationToken = default);
    Task RemoveCompilationLogAsync(TKey id, CancellationToken cancellationToken = default);
}
