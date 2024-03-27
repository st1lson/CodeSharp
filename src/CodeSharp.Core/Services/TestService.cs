using CodeSharp.Core.Contracts;
using CodeSharp.Core.Executors;
using CodeSharp.Core.Models;

namespace CodeSharp.Core.Services;

public class TestService<TTest> : TestService<TTest, TestLog, Guid>
    where TTest : ITest<Guid>
{
    public TestService(
        ITestStore<TTest, Guid> testingStore,
        ITestExecutor<TestLog> testExecutor,
        ITestLogStore<TestLog, Guid> testLogStore) : base(testingStore, testExecutor, testLogStore)
    {
    }
}

public class TestService<TTest, TTestLog, TKey> : ITestService<TTest, TTestLog, TKey>
    where TTest : ITest<TKey>
    where TTestLog : ITestLog<TKey>
{
    private readonly ITestStore<TTest, TKey> _testingStore;
    private readonly ITestLogStore<TTestLog, TKey> _testLogStore;
    private readonly ITestExecutor<TTestLog> _testExecutor;

    public TestService(ITestStore<TTest, TKey> testingStore, ITestExecutor<TTestLog> testExecutor, ITestLogStore<TTestLog, TKey> testLogStore)
    {
        _testingStore = testingStore;
        _testExecutor = testExecutor;
        _testLogStore = testLogStore;
    }

    public Task AddTestAsync(TTest test, CancellationToken cancellationToken = default)
    {
        return _testingStore.CreateAsync(test, cancellationToken);
    }

    public Task DeleteTestAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return _testingStore.DeleteAsync(id, cancellationToken);
    }

    public async Task<TTestLog> ExecuteTestAsync(TTest test, string code, CancellationToken cancellationToken = default)
    {
        if (test is null)
        {
            throw new ArgumentNullException(nameof(test));
        }

        var testingResult = await _testExecutor.TestAsync(code, test.Tests, cancellationToken);

        await _testLogStore.CreateAsync(testingResult, cancellationToken);

        return testingResult;
    }

    public async Task<TTestLog> ExecuteTestByIdAsync(TKey id, string code, CancellationToken cancellationToken = default)
    {
        var test = await _testingStore.GetByIdAsync(id, cancellationToken) ?? throw new Exception("Test not found");

        return await ExecuteTestAsync(test, code, cancellationToken);
    }

    public Task<TTest?> GetTestAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return _testingStore.GetByIdAsync(id, cancellationToken);
    }

    public Task<IList<TTest>> GetTestsAsync(CancellationToken cancellationToken = default)
    {
        return _testingStore.GetAllAsync(cancellationToken);
    }

    public Task UpdateTestAsync(TTest test, CancellationToken cancellationToken = default)
    {
        return _testingStore.UpdateAsync(test, cancellationToken);
    }
}
