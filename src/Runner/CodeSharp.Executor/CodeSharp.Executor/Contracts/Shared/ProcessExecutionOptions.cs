namespace CodeSharp.Executor.Contracts.Shared;

public record ProcessExecutionOptions(
    string FileName,
    string Arguments,
    bool RedirectStandardOutput = true,
    bool RedirectStandardError = true,
    bool CreateNoWindow = true,
    long? MaxRamUsageInMB = null,
    TimeSpan? MaxDuration = null,
    Queue<string>? Inputs = null);