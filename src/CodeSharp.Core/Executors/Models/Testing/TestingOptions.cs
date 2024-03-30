using CodeSharp.Core.Executors.Models.Shared;

namespace CodeSharp.Core.Executors.Models.Testing;

public class TestingOptions : ExecutionOptions
{
    public TimeSpan MaxTestingTime { get; set; } = TimeSpan.FromSeconds(30);

    public static TestingOptions Default => new();
}
