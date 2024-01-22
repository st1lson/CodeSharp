using System.Text.Json;
using CodeSharp.Core.Executors.Models.Shared;

namespace CodeSharp.Core.Executors.Models.Testing;

public class TestingResponse
{
    public bool Success => TestResults.All(tr => tr.Passed);
    public required IList<TestingResult> TestResults { get; init; } = new List<TestingResult>();
    public required CodeAnalysisReport CodeReport { get; init; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }
}