namespace CodeSharp.Core.Executors;

public interface ITestExecutor<TResponse>
{
    Task<TResponse> TestAsync(string code, string testsCode, CancellationToken cancellationToken = default);
    Task<TResponse> TestFileAsync(string filePath, string testsCode, CancellationToken cancellationToken = default);
}
