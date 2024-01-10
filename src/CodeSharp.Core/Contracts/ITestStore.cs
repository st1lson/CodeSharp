namespace CodeSharp.Core.Contracts;

public interface ITestStore<TItem, TKey> where TItem : ITest<TKey>
{
    Task CreateAsync(TItem item, CancellationToken cancellationToken = default);
    Task UpdateAsync(TItem item, CancellationToken cancellationToken = default);
    Task DeleteAsync(TKey key, CancellationToken cancellationToken = default);
    Task<TItem?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<IList<TItem>> GetAllAsync(CancellationToken cancellationToken = default);
}