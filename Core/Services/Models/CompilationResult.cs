using System.Text;
using System.Text.Json.Serialization;

namespace Core.Services.Models;

public class CompilationResult
{
    public bool Success { get; set; }
    public TimeSpan TimeTaken { get; set; }
    public string Error { get; set; } = string.Empty;
    public string Output { get; set; } = string.Empty;

    public override string ToString()
    {
        var builder = new StringBuilder();

        builder.AppendLine($"Compilation {(Success ? "successful" : "failed")}!\n");
        builder.AppendLine($"Time taken: {TimeTaken.TotalSeconds} seconds\n");
        builder.AppendLine($"Output:\n{Output}\n");
        builder.AppendLine($"Error:\n{Error}");

        return builder.ToString();
    }
}