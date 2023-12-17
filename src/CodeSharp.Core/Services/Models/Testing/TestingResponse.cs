using System.Text.Json;
using CodeSharp.Core.Services.Models.Shared;

namespace CodeSharp.Core.Services.Models.Testing;

public class TestingResponse : AnalyzableResponse
{
    public bool Success => TestResults.All(tr => tr.Passed);
    public IList<TestResult> TestResults { get; set; } = new List<TestResult>();

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }
}