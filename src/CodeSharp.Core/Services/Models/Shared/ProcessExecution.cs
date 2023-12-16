namespace Core.Services.Models.Shared;

public class ProcessExecution
{
    public bool Success { get; set; }
    public TimeSpan Duration { get; set; }
    public string? Error { get; set; }
    public string? Output { get; set; }
}
