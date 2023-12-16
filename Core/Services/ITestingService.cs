using Core.Services.Models.Testing;

namespace Core.Services;

public interface ITestingService
{
    Task<TestingResponse> TestAsync(string code, string testsCode, CancellationToken cancellationToken = default);
    Task<TestingResponse> TestFileAsync(string filePath, string testsCode, CancellationToken cancellationToken = default);
}
