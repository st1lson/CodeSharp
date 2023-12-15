namespace CodeSharp.Executor.Contracts.Internal;

public class CodeAnalysisReport
{
    public IList<CodeAnalysisIssue> Errors { get; set; } = new List<CodeAnalysisIssue>();
    public IList<CodeAnalysisIssue> CodeAnalysisIssues { get; set; } = new List<CodeAnalysisIssue>();
}