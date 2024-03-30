namespace CodeSharp.Executor.Contracts.Shared;

public abstract class ExecutionOptions
{
    public int MaxCompilationTime { get; init; }
    public int MaxRamUsage { get; init; }
}
