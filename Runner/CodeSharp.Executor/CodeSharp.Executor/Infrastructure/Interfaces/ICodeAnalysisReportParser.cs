using CodeSharp.Executor.Contracts.Internal;

namespace CodeSharp.Executor.Infrastructure.Interfaces;

public interface ICodeAnalysisReportParser
{
    Task<CodeAnalysisReport> ParseCodeAnalysisReportAsync(CancellationToken cancellationToken = default);
}