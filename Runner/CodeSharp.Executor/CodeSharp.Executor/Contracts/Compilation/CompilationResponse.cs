using CodeSharp.Executor.Contracts.Internal;

namespace CodeSharp.Executor.Contracts.Compilation;

public class CompilationResponse : CodeAnalysisResponse
{
    public virtual bool Success { get; set; }
    public TimeSpan Duration { get; set; }
}