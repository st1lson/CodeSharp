using CodeSharp.Executor.Contracts.Internal;

namespace CodeSharp.Executor.Infrastructure.Interfaces;

public interface ICodeAnalysisReportParser
{
    Task<CodeAnalysisResponse> ParseCodeAnalysisReportAsync(CancellationToken cancellationToken = default);
}