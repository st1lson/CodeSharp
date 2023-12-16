namespace CodeSharp.Executor.Contracts.Internal;

public class CodeMetricsReport
{
    public int MaintainabilityIndex { get; set; }
    public int CyclomaticComplexity { get; set; }
    public int ClassCoupling { get; set; }
    public int DepthOfInheritance { get; set; }
    public int SourceLines { get; set; }
    public int ExecutableLines { get; set; }

    public IList<CodeMetricsReport> NestedMetrics { get; set; } = new List<CodeMetricsReport>();
    public SourceInfo? SourceInfo { get; set; }
}