namespace CodeSharp.Core.Contracts;

public interface ICompilationLogStore<TCompilationLog, TKey> where TCompilationLog : ICompilationLog<TKey>
{
    Task CreateAsync(TCompilationLog item, CancellationToken cancellationToken = default);
    Task<TCompilationLog?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<IList<TCompilationLog>> GetAllAsync(CancellationToken cancellationToken = default);
    Task RemoveAsync(TKey id, CancellationToken cancellationToken = default);
}
