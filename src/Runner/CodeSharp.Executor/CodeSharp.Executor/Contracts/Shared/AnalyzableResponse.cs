namespace CodeSharp.Executor.Contracts.Shared;

public abstract class AnalyzableResponse
{
    public CodeAnalysisReport? CodeReport { get; set; }
}