namespace CodeSharp.Core.Services.Models.Testing;

public sealed record TestResult
{
    public required string TestName { get; init; }
    public bool Passed { get; init; }
    public double ExecutionTime { get; init; }
    public string? ErrorMessage { get; init; }
}