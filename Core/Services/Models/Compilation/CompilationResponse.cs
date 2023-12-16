using System.Text.Json;
using Core.Services.Models.Shared;

namespace Core.Services.Models.Compilation;

public class CompilationResponse : AnalyzableResponse
{
    public bool Success { get; set; }
    public TimeSpan Duration { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}