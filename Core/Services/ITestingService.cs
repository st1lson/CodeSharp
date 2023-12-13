using Core.Services.Models;

namespace Core.Services;

public interface ITestingService
{
    Task<TestingResult> TestAsync(string code, string testsCode, CancellationToken cancellationToken = default);
    Task<TestingResult> TestFileAsync(string filePath, string testsCode, CancellationToken cancellationToken = default);
}
