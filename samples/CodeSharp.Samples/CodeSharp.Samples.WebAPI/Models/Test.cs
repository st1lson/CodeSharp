namespace CodeSharp.Samples.WebAPI.Models;

public record Test
{
    public Guid Id { get; init; }
    public required string TestsCode { get; init; }
    public required string InitialUserCode { get; init; }
    public required string Description { get; init; }
}
