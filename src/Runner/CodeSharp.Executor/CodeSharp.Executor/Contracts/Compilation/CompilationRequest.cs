namespace CodeSharp.Executor.Contracts.Compilation;

public class CompilationRequest
{
    public required string Code { get; set; }
    public bool Run { get; set; }
}