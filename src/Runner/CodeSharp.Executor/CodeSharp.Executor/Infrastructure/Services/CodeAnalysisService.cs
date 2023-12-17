using CodeSharp.Executor.Contracts.Shared;
using CodeSharp.Executor.Infrastructure.Interfaces;

namespace CodeSharp.Executor.Infrastructure.Services;

public class CodeAnalysisService : ICodeAnalysisService
{
    private readonly ICodeAnalysisReportParser _codeAnalysisReportParser;

    public CodeAnalysisService(ICodeAnalysisReportParser codeAnalysisReportParser)
    {
        _codeAnalysisReportParser = codeAnalysisReportParser;
    }

    public Task<CodeAnalysisReport> AnalyzeAsync(CancellationToken cancellationToken = default)
    {
        return _codeAnalysisReportParser.ParseCodeAnalysisReportAsync(cancellationToken);
    }
}