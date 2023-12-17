namespace CodeSharp.Samples.WebAPI.Models.Requests;

public record TestingRequest
{
    public Guid TestId { get; init; }
    public required string Code { get; init; }
}
