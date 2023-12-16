using Core.Services.Models.Shared;
using System.Text.Json;

namespace Core.Services.Models.Compilation;

public class CompilationResponse : AnalyzableResponse
{
    public bool Success { get; set; }
    public TimeSpan Duration { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }
}