namespace CodeSharp.Executor.Contracts.Internal;

public record CodeAnalysisIssue
{
    public required string FileName { get; init; }
    public required string Position { get; init; }
    public required string Code { get; init; }
    public required string Severity { get; init; }
    public required  string Message { get; init; }
}