using CodeSharp.Core.Contracts;

namespace CodeSharp.Core.Services;

public class TestService<TTest> : TestService<TTest, Guid>
    where TTest : ITest<Guid>
{
    public TestService(ITestStore<TTest, Guid> testingStore) : base(testingStore)
    {
    }
}

public class TestService<TTest, TKey> : ITestService<TTest, TKey>
    where TTest : ITest<TKey>
{
    private readonly ITestStore<TTest, TKey> _testingStore;

    public TestService(ITestStore<TTest, TKey> testingStore)
    {
        _testingStore = testingStore;
    }

    public Task AddTestAsync(TTest test, CancellationToken cancellationToken = default)
    {
        return _testingStore.CreateAsync(test, cancellationToken);
    }

    public Task DeleteTestAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return _testingStore.DeleteAsync(id, cancellationToken);
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
