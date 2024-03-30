namespace CodeSharp.Executor.Contracts.Compilation;

public class CompilationRequest
{
    public required string Code { get; set; }
    public required CompilationOptions Options { get; set; }
}