using CodeSharp.Executor.Contracts.Internal;

namespace CodeSharp.Executor.Infrastructure.Interfaces;

public interface ICodeAnalysisService
{
    Task<CodeAnalysisResponse> AnalyseAsync(CancellationToken cancellationToken = default);
}