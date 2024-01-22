namespace CodeSharp.Core.Executors.Models.Testing;

public class TestingRequest
{
    public required string CodeToTest { get; init; }
    public required string TestsCode { get; init; }
}