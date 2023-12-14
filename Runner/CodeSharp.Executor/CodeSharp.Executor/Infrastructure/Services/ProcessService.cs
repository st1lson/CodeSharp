using CodeSharp.Executor.Contracts.Internal;
using CodeSharp.Executor.Infrastructure.Interfaces;
using System.Diagnostics;

namespace CodeSharp.Executor.Infrastructure.Services;

public class ProcessService : IProcessService
{
    public async Task<ProcessExecution> ExecuteProcessAsync(
        ProcessExecutionOptions executionOptions,
        CancellationToken cancellationToken = default)
    {
        var result = new ProcessExecution();

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        try
        {
            using var process = new Process();
            process.StartInfo.FileName = executionOptions.FileName;
            process.StartInfo.Arguments = executionOptions.Arguments;
            process.StartInfo.RedirectStandardOutput = executionOptions.RedirectStandardOutput;
            process.StartInfo.RedirectStandardError = executionOptions.RedirectStandardError;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = executionOptions.CreateNoWindow;

            var outputWriter = new StringWriter();
            var errorWriter = new StringWriter();

            process.OutputDataReceived += (_, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    outputWriter.WriteLine($"[Output] {e.Data}");
                }
            };

            process.ErrorDataReceived += (_, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    errorWriter.WriteLine($"[Error] {e.Data}");
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            await process.WaitForExitAsync(cancellationToken);

            stopwatch.Stop();
            result.Duration = stopwatch.Elapsed;

            result.Success = (process.ExitCode == 0);
            result.Output = outputWriter.ToString();
            result.Error = errorWriter.ToString();
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Error = $"An error occurred: {ex.Message}";
        }

        return result;
    }
}