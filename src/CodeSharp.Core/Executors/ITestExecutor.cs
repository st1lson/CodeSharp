using CodeSharp.Core.Services.Models.Testing;

namespace CodeSharp.Core.Services;

public interface ITestExecutor<TResponse> where TResponse : TestingResponse
{
    Task<TResponse> TestAsync(string code, string testsCode, CancellationToken cancellationToken = default);
    Task<TResponse> TestFileAsync(string filePath, string testsCode, CancellationToken cancellationToken = default);
}
