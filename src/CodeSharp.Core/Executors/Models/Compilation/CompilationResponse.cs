using CodeSharp.Core.Executors.Models.Shared;
using System.Text.Json;

namespace CodeSharp.Core.Executors.Models.Compilation;

public class CompilationResponse
{
    public bool CompiledSuccessfully { get; init; }
    public bool? ExecutedSuccessfully { get; init; }
    public TimeSpan CompilationDuration { get; init; }
    public TimeSpan? ExecutionDuration { get; init; }
    public string? Output { get; init; }
    public required CodeAnalysisReport CodeReport { get; init; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }
}