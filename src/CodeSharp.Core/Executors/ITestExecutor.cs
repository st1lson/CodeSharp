using CodeSharp.Core.Executors.Models.Testing;

namespace CodeSharp.Core.Executors;

public interface ITestExecutor<TResponse>
{
    Task<TResponse> TestAsync(string code, string testsCode, TestingOptions? options = default, CancellationToken cancellationToken = default);
    Task<TResponse> TestFileAsync(string filePath, string testsCode, TestingOptions? options = default, CancellationToken cancellationToken = default);
}
