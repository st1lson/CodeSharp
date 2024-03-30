using CodeSharp.Executor.Contracts.Shared;

namespace CodeSharp.Executor.Contracts.Compilation;

public class CompilationOptions : ExecutionOptions
{
    public bool Run { get; init; }
    public TimeSpan MaxExecutionTime { get; init; } = TimeSpan.FromSeconds(15);
    public Queue<string>? Inputs { get; init; }

    public static CompilationOptions Default => new();
}
