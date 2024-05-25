namespace CodeSharp.Core.Contracts;

public interface ICompilationLogStore<TCompilationLog, TKey> where TCompilationLog : ICompilationLog<TKey>
{
    Task<IList<TCompilationLog>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TCompilationLog?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<TCompilationLog> CreateAsync(TCompilationLog item, CancellationToken cancellationToken = default);
    Task<TCompilationLog?> RemoveAsync(TKey id, CancellationToken cancellationToken = default);
}
