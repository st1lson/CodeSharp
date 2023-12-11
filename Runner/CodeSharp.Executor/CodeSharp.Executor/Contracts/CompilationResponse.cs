namespace CodeSharp.Executor.Contracts;

public class CompilationResponse
{
    public virtual bool Success { get; set; }
    public TimeSpan Duration { get; set; }
    public string Error { get; set; } = string.Empty;
    public string Output { get; set; } = string.Empty;
}