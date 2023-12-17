using CodeSharp.Core.Services.Models.Shared;
using System.Text.Json;

namespace CodeSharp.Core.Services.Models.Compilation;

public class CompilationResponse
{
    public bool Success { get; set; }
    public TimeSpan Duration { get; set; }
    public required CodeAnalysisResponse CodeReport { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }
}