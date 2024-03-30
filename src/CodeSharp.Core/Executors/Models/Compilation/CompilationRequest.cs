namespace CodeSharp.Core.Executors.Models.Compilation;

public class CompilationRequest
{
    public required string Code { get; set; }
    public CompilationOptions Options { get; set; }
}