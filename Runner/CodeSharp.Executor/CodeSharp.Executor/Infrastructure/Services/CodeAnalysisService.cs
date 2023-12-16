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

    public async Task<CodeAnalysisResponse> AnalyzeAsync(CancellationToken cancellationToken = default)
    {
        var codeReport = await _codeAnalysisReportParser.ParseCodeAnalysisReportAsync(cancellationToken);

        //var metricsReport = _codeMetricsReport.Parse();

        return new CodeAnalysisResponse
        {
            CodeAnalysis = codeReport
        };
    }
}