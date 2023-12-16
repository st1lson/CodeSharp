namespace CodeSharp.Executor.Contracts.Shared;

public abstract class AnalyzableResponse
{
    public CodeAnalysisResponse CodeReport { get; set; }
}