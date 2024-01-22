using CodeSharp.Core.Contracts;

namespace CodeSharp.Core.Services;

public class TestLogService<TTestLog, TKey> : ITestLogService<TTestLog, TKey> where TTestLog : ITestLog<TKey>
{
    private readonly ITestLogStore<TTestLog, TKey> _testLogStore;

    public TestLogService(ITestLogStore<TTestLog, TKey> testLogStore)
    {
        _testLogStore = testLogStore;
    }

    public Task AddTestLogAsync(TTestLog testLog, CancellationToken cancellationToken = default)
    {
        return _testLogStore.CreateAsync(testLog, cancellationToken);
    }

    public Task<TTestLog?> GetTestLogAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return _testLogStore.GetByIdAsync(id, cancellationToken);
    }

    public Task<IList<TTestLog>> GetTestLogsAsync(CancellationToken cancellationToken = default)
    {
        return _testLogStore.GetAllAsync(cancellationToken);
    }

    public Task RemoveTestLogAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return _testLogStore.RemoveAsync(id, cancellationToken);
    }
}
