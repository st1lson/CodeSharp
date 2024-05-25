using CodeSharp.Core.Contracts;

namespace CodeSharp.Core.Services;

public class TestLogService<TTestLog> : TestLogService<TTestLog, Guid> where TTestLog : ITestLog<Guid>
{
    public TestLogService(ITestLogStore<TTestLog, Guid> testLogStore) : base(testLogStore)
    {
    }
}

public class TestLogService<TTestLog, TKey> : ITestLogService<TTestLog, TKey> where TTestLog : ITestLog<TKey>
{
    private readonly ITestLogStore<TTestLog, TKey> _testLogStore;

    public TestLogService(ITestLogStore<TTestLog, TKey> testLogStore)
    {
        _testLogStore = testLogStore;
    }

    public Task<TTestLog> AddTestLogAsync(TTestLog testLog, CancellationToken cancellationToken = default)
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

    public Task<TTestLog?> RemoveTestLogAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return _testLogStore.RemoveAsync(id, cancellationToken);
    }
}
