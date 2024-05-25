using CodeSharp.Core.Contracts;

namespace CodeSharp.Core.Services;

public interface ITestLogService<TTestLog, TKey> where TTestLog : ITestLog<TKey>
{
    Task<IList<TTestLog>> GetTestLogsAsync(CancellationToken cancellationToken = default);
    Task<TTestLog?> GetTestLogAsync(TKey id, CancellationToken cancellationToken = default);
    Task<TTestLog> AddTestLogAsync(TTestLog testLog, CancellationToken cancellationToken = default);
    Task<TTestLog?> RemoveTestLogAsync(TKey id, CancellationToken cancellationToken = default);
}
