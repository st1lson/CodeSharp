namespace CodeSharp.Executor.Contracts.Shared;

public class CodeAnalysisResponse
{
    public int CodeGrade { get; set; }
    public CodeAnalysisReport? CodeAnalysis { get; set; }
    public CodeMetricsReport? CodeMetrics { get; set; }
}