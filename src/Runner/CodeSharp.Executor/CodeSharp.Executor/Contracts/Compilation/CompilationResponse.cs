using CodeSharp.Executor.Contracts.Shared;

namespace CodeSharp.Executor.Contracts.Compilation;

public class CompilationResponse : AnalyzableResponse
{
    public bool Success { get; set; }
    public TimeSpan Duration { get; set; }
    public string? Output { get; set; }
}