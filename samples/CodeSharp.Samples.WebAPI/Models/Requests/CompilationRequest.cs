namespace CodeSharp.Samples.WebAPI.Models.Requests;

public record CompilationRequest
{
    public required string Code { get; init; }
}
