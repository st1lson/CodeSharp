namespace CodeSharp.Core.Contracts;

public interface ITestStore<TTest, TKey> where TTest : ITest<TKey>
{
    Task<IList<TTest>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TTest?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<TTest> CreateAsync(TTest item, CancellationToken cancellationToken = default);
    Task<TTest> UpdateAsync(TTest item, CancellationToken cancellationToken = default);
    Task<TTest?> DeleteAsync(TKey key, CancellationToken cancellationToken = default);
}