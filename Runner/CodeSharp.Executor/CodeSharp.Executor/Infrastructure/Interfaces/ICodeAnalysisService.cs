using CodeSharp.Executor.Contracts.Shared;

namespace CodeSharp.Executor.Infrastructure.Interfaces;

public interface ICodeAnalysisService
{
    Task<CodeAnalysisResponse> AnalyzeAsync(CancellationToken cancellationToken = default);
}