namespace CodeSharp.Executor.Contracts.Shared;

public record CodeAnalysisIssue
{
    public int Line { get; init; }
    public int Column { get; init; }
    public required string Code { get; init; }
    public required string Severity { get; init; }
    public required string Message { get; init; }
}