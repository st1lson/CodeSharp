namespace CodeSharp.Executor.Contracts.Internal;

public record CodeAnalysisIssue
{
    public int Line { get; set; }
    public int Column { get; set; }
    public required string Code { get; init; }
    public required string Severity { get; init; }
    public required string Message { get; init; }
}