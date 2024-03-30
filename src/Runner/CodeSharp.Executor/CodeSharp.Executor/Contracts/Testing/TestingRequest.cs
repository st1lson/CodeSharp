namespace CodeSharp.Executor.Contracts.Testing;

public class TestingRequest
{
    public required string CodeToTest { get; init; }
    public required string TestsCode { get; init; }
    public required TestingOptions Options { get; init; }
}