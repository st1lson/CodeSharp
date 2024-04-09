namespace CodeSharp.Samples.WebAPI.Models.Requests;

public record CompilationRequest
{
    public required string Code { get; init; }
    public bool Run { get; init; }
    public int? MaxCompilationTime { get; init; }
    public int? MaxRamUsage { get; init; }
    public int MaxExecutionTime { get; init; }
    public Queue<string>? Inputs { get; init; }
}
