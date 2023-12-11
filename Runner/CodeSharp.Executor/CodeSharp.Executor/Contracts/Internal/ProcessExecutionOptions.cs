namespace CodeSharp.Executor.Contracts.Internal;

public record ProcessExecutionOptions(
    string FileName,
    string Arguments,
    bool RedirectStandardOutput = true,
    bool RedirectStandardError = true,
    bool CreateNoWindow = true,
    long? MaxRamUsage = null,
    TimeSpan? MaxDuration = null);