namespace CodeSharp.Core.Contracts;

public interface ITestLogStore<TTestLog, TKey> where TTestLog : ITestLog<TKey>
{
    Task<IList<TTestLog>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TTestLog?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task CreateAsync(TTestLog item, CancellationToken cancellationToken = default);
    Task RemoveAsync(TKey id, CancellationToken cancellationToken = default);
}
