namespace Core.Services.Models.Shared;

public record ProcessExecutionOptions(
    string FileName,
    string Arguments,
    bool RedirectStandardOutput = true,
    bool RedirectStandardError = true,
    bool CreateNoWindow = true,
    long? MaxRamUsage = null,
    TimeSpan? MaxDuration = null);