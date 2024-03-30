using CodeSharp.Core.Executors.Models.Shared;

namespace CodeSharp.Core.Executors.Models.Compilation;

public class CompilationOptions : ExecutionOptions
{
    public bool Run { get; init; }
    public TimeSpan MaxExecutionTime { get; init; } = TimeSpan.FromSeconds(15);
    public Queue<string>? Inputs { get; init; }

    public static CompilationOptions Default => new();
}
