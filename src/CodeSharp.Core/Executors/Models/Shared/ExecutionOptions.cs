namespace CodeSharp.Core.Executors.Models.Shared;

public abstract class ExecutionOptions
{
    public int MaxCompilationTime { get; init; }
    public int MaxRamUsage { get; init; }
}
