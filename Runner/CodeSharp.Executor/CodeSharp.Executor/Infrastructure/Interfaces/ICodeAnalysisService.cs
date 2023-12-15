using CodeSharp.Executor.Contracts.Internal;

namespace CodeSharp.Executor.Infrastructure.Interfaces;

public interface ICodeAnalysisService
{
    Task<CodeAnalysisResponse> AnalyzeAsync(CancellationToken cancellationToken = default);
}