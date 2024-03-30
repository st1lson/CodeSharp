namespace CodeSharp.Executor.Contracts.Shared;

public abstract class ExecutionOptions
{
    public TimeSpan? MaxCompilationTime { get; init; }
    public int? MaxRamUsage { get; init; }
}
