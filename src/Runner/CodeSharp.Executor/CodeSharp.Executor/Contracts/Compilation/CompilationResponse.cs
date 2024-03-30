using CodeSharp.Executor.Contracts.Shared;

namespace CodeSharp.Executor.Contracts.Compilation;

public class CompilationResponse : AnalyzableResponse
{
    public bool CompiledSuccessfully { get; set; }
    public bool? ExecutedSuccessfully { get; set; }
    public TimeSpan CompilationDuration { get; set; }
    public TimeSpan? ExecutionDuration { get; set; }
    public string? Output { get; set; }
}