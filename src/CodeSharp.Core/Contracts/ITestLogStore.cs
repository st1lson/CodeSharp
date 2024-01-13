namespace CodeSharp.Core.Contracts;

public interface ITestLogStore<TTestLog, TKey> where TTestLog : ITestLog<TKey>
{
    Task CreateAsync(TTestLog item, CancellationToken cancellationToken = default);
    Task<TTestLog?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<IList<TTestLog>> GetAllAsync(CancellationToken cancellationToken = default);
}
