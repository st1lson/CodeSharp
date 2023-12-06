namespace CodeSharp.Executor.Contracts;

public class CompilationResponse
{
    public bool Success { get; set; }
    public TimeSpan TimeTaken { get; set; }
    public string Error { get; set; } = string.Empty;
    public string Output { get; set; } = string.Empty;
}