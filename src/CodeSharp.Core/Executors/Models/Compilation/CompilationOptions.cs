using CodeSharp.Core.Executors.Models.Shared;

namespace CodeSharp.Core.Executors.Models.Compilation;

public class CompilationOptions : ExecutionOptions
{
    public bool Run { get; init; }
    public int MaxExecutionTime { get; init; } = 15;

    public static CompilationOptions Default => new();
}
