using CodeSharp.Core.Services.Models.Shared;
using System.Text.Json;

namespace CodeSharp.Core.Services.Models.Testing;

public class TestingResponse
{
    public bool Success => TestResults.All(tr => tr.Passed);
    public IList<TestResult> TestResults { get; set; } = new List<TestResult>();
    public required CodeAnalysisResponse CodeReport { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }
}