using CodeSharp.Core.Contracts;

namespace CodeSharp.Core.Services;

public interface ITestService<TTest> :
    ITestService<TTest, Guid>
    where TTest : ITest
{
}

public interface ITestService<TTest, TKey> where TTest : ITest<TKey>
{
    Task<IList<TTest>> GetTestsAsync(CancellationToken cancellationToken = default);
    Task<TTest?> GetTestAsync(TKey id, CancellationToken cancellationToken = default);
    Task AddTestAsync(TTest test, CancellationToken cancellationToken = default);
    Task UpdateTestAsync(TTest test, CancellationToken cancellationToken = default);
    Task DeleteTestAsync(TKey id, CancellationToken cancellationToken = default);
}
