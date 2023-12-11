using CodeSharp.Executor.Infrastructure.Interfaces;
using CodeSharp.Executor.Options;
using Microsoft.Extensions.Options;

namespace CodeSharp.Executor.Infrastructure.Services;

public sealed class FileService : IFileService
{
    private readonly ApplicationOptions _applicationOptions;

    public FileService(IOptions<ApplicationOptions> applicationOptions)
    {
        _applicationOptions = applicationOptions.Value;
    }
    
    public Task ReplaceProgramFileAsync(string newCode, CancellationToken cancellationToken = default)
    {
        var codeFilePath = _applicationOptions.ConsoleFilePath;
        
        return File.WriteAllTextAsync(codeFilePath, newCode, cancellationToken);
    }

    public Task ReplaceTestsFileAsync(string newTests, CancellationToken cancellationToken = default)
    {
        var testFilePath = _applicationOptions.TestFilePath;
        
        return File.WriteAllTextAsync(testFilePath, newTests, cancellationToken);
    }
}