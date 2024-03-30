using CodeSharp.Executor.Contracts.Shared;

namespace CodeSharp.Executor.Contracts.Compilation;

public class CompilationOptions : ExecutionOptions
{
    public bool Run { get; init; }
    public int MaxExecutionTime { get; init; } = 15;

    public static CompilationOptions Default => new();
}
