namespace CodeSharp.Core.Executors.Models.Shared;

public abstract class ExecutionOptions
{
    public TimeSpan? MaxCompilationTime { get; init; }
    public int? MaxRamUsage { get; init; }
}
