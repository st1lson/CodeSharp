namespace CodeSharp.Core.Services.Models.Shared;

public abstract class AnalyzableResponse
{
    public required CodeAnalysisResponse CodeReport { get; set; }
}