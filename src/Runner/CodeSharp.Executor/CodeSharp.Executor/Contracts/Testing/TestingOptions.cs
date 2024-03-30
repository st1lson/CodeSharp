using CodeSharp.Executor.Contracts.Shared;

namespace CodeSharp.Executor.Contracts.Testing;

public class TestingOptions : ExecutionOptions
{
    public static TestingOptions Default => new();
}