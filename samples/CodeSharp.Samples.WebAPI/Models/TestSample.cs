namespace CodeSharp.Samples.WebAPI.Models;

public record TestSample
{
    public Guid Id { get; init; }
    public required string InitialUserCode { get; init; }
    public required string Description { get; init; }
}
