using CodeSharp.Executor.Contracts.Shared;

namespace CodeSharp.Executor.Infrastructure.Interfaces;

public interface ICodeAnalysisReportParser
{
    Task<CodeAnalysisReport> ParseCodeAnalysisReportAsync(CancellationToken cancellationToken = default);
}