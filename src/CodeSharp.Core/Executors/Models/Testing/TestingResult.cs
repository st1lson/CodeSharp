namespace CodeSharp.Core.Executors.Models.Testing;

public sealed record TestingResult(string TestName, bool Passed, double ExecutionTime, string? ErrorMessage);