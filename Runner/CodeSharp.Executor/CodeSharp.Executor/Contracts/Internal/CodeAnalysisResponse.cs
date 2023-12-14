namespace CodeSharp.Executor.Contracts.Internal;

public class CodeAnalysisResponse
{
    public int CodeGrade { get; init; }
    public IList<CodeAnalysisIssue> Errors { get; set; } = new List<CodeAnalysisIssue>();
    public IList<CodeAnalysisIssue> CodeAnalysisIssues { get; set; } = new List<CodeAnalysisIssue>();
}