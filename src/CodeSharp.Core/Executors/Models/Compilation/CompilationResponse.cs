using System.Text.Json;
using CodeSharp.Core.Executors.Models.Shared;

namespace CodeSharp.Core.Executors.Models.Compilation;

public class CompilationResponse
{
    public bool Success { get; }
    public TimeSpan Duration { get; init; }
    public string? Output { get; init; }
    public required CodeAnalysisReport CodeReport { get; init; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }
}