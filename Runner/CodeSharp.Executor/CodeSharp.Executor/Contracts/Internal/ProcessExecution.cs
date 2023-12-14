namespace CodeSharp.Executor.Contracts.Internal;

public class ProcessExecution
{
    public bool Success { get; set; }
    public TimeSpan Duration { get; set; }
    public string? Error { get; set; }
    public string? Output { get; set; }
}
