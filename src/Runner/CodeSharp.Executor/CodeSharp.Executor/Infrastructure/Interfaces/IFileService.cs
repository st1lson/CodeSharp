namespace CodeSharp.Executor.Infrastructure.Interfaces;

public interface IFileService
{
    Task ReplaceProgramFileAsync(string newCode, CancellationToken cancellationToken = default);
    Task ReplaceCodeToTestFileAsync(string newCode, CancellationToken cancellationToken = default);
    Task ReplaceTestsFileAsync(string newTests, CancellationToken cancellationToken = default);
}