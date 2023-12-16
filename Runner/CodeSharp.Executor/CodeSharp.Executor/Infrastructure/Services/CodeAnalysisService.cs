using CodeSharp.Executor.Contracts.Shared;
using CodeSharp.Executor.Infrastructure.Interfaces;

namespace CodeSharp.Executor.Infrastructure.Services;

public class CodeAnalysisService : ICodeAnalysisService
{
    private readonly ICodeAnalysisReportParser _codeAnalysisReportParser;
    private readonly ICodeMetricsReportParser _codeMetricsReport;

    public CodeAnalysisService(ICodeAnalysisReportParser codeAnalysisReportParser, ICodeMetricsReportParser codeMetricsReport)
    {
        _codeAnalysisReportParser = codeAnalysisReportParser;
        _codeMetricsReport = codeMetricsReport;
    }

    public async Task<CodeAnalysisResponse> AnalyzeAsync(CancellationToken cancellationToken = default)
    {
        var codeReport = await _codeAnalysisReportParser.ParseCodeAnalysisReportAsync(cancellationToken);

        var metricsReport = _codeMetricsReport.Parse();

        return new CodeAnalysisResponse
        {
            CodeAnalysis = codeReport,
            CodeMetrics = metricsReport,
            CodeGrade = Random.Shared.Next(100)
        };
    }
}