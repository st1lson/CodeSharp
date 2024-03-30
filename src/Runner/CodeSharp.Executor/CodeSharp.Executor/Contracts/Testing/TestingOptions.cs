using CodeSharp.Executor.Contracts.Shared;

namespace CodeSharp.Executor.Contracts.Testing;

public class TestingOptions : ExecutionOptions
{
    public TimeSpan MaxTestingTime { get; set; } = TimeSpan.FromSeconds(30);

    public static TestingOptions Default => new();
}