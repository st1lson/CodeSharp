using CodeSharp.Core.Contracts;

namespace CodeSharp.Core.Services;

public interface ITestLogService<TTestLog, TKey> where TTestLog : ITestLog<TKey>
{
    Task<IList<TTestLog>> GetTestLogsAsync(CancellationToken cancellationToken = default);
    Task<TTestLog?> GetTestLogAsync(TKey id, CancellationToken cancellationToken = default);
    Task AddTestLogAsync(TTestLog testLog, CancellationToken cancellationToken = default);
    Task RemoveTestLogAsync(TKey id, CancellationToken cancellationToken = default);
}
