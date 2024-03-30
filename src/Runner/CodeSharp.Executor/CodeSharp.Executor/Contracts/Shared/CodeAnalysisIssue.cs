namespace CodeSharp.Executor.Contracts.Shared;

public record CodeAnalysisIssue
{
    public int Line { get; init; }
    public int Column { get; init; }
    public string? Code { get; init; }
    public string? Severity { get; init; }
    public required string Message { get; init; }
}