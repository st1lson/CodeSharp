using CodeSharp.Executor.Contracts.Shared;

namespace CodeSharp.Executor.Infrastructure.Interfaces;

public interface ICodeAnalysisService
{
    Task<CodeAnalysisReport> AnalyzeAsync(CancellationToken cancellationToken = default);
}