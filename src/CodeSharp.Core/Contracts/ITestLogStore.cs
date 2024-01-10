namespace CodeSharp.Core.Contracts;

public interface ITestLogStore<TTestLog, TKey> where TTestLog : ITest<TKey>
{
    Task CreateAsync(TTestLog item, CancellationToken cancellationToken = default);
    Task UpdateAsync(TTestLog item, CancellationToken cancellationToken = default);
    Task DeleteAsync(TKey key, CancellationToken cancellationToken = default);
    Task<TTestLog?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<IList<TTestLog>> GetAllAsync(CancellationToken cancellationToken = default);
}
