using System.Text.Json;
using Core.Services.Models.Shared;

namespace Core.Services.Models.Testing;

public class TestingResponse : AnalyzableResponse
{
    public bool Success => TestResults.All(tr => tr.Passed);
    public IList<TestResult> TestResults { get; set; } = new List<TestResult>();

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}