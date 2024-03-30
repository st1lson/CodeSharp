using CodeSharp.Core.Executors.Models.Shared;
using System.Text.Json;

namespace CodeSharp.Core.Executors.Models.Testing;

public class TestingResponse
{
    public bool Passed => TestResults.All(tr => tr.Passed) && CompiledSuccessfully && TestedSuccessfully;
    public bool CompiledSuccessfully { get; set; }
    public bool TestedSuccessfully { get; set; }
    public TimeSpan CompilationDuration { get; set; }
    public TimeSpan? TestingDuration { get; set; }
    public required IList<TestingResult> TestResults { get; init; } = new List<TestingResult>();
    public required CodeAnalysisReport CodeReport { get; init; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }
}