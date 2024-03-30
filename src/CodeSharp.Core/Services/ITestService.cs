using CodeSharp.Core.Contracts;
using CodeSharp.Core.Executors.Models.Testing;

namespace CodeSharp.Core.Services;

public interface ITestService<TTest, TTestLog> :
    ITestService<TTest, TTestLog, Guid>
    where TTest : ITest<Guid>
    where TTestLog : ITestLog<Guid>
{
}

public interface ITestService<TTest, TTestLog, TKey>
    where TTest : ITest<TKey>
    where TTestLog : ITestLog<TKey>
{
    Task<TTestLog> ExecuteTestAsync(TTest test, string code, TestingOptions? options = default, CancellationToken cancellationToken = default);
    Task<TTestLog> ExecuteTestByIdAsync(TKey id, string code, TestingOptions? options = default, CancellationToken cancellationToken = default);
    Task<IList<TTest>> GetTestsAsync(CancellationToken cancellationToken = default);
    Task<TTest?> GetTestAsync(TKey id, CancellationToken cancellationToken = default);
    Task AddTestAsync(TTest test, CancellationToken cancellationToken = default);
    Task UpdateTestAsync(TTest test, CancellationToken cancellationToken = default);
    Task DeleteTestAsync(TKey id, CancellationToken cancellationToken = default);
}
