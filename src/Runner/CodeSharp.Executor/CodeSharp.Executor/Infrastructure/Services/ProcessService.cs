using CodeSharp.Executor.Contracts.Shared;
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

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        if (executionOptions.MaxDuration.HasValue)
        {
            cts.CancelAfter(executionOptions.MaxDuration.Value);
        }
        var linkedToken = cts.Token;

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
                    outputWriter.WriteLine(e.Data);
                }
            };

            process.ErrorDataReceived += (_, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    errorWriter.WriteLine(e.Data);
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            var memoryMonitorTask = Task.Run(() => MemoryMonitor(process, executionOptions.MaxRamUsage ?? long.MaxValue, linkedToken), cancellationToken);

            await process.WaitForExitAsync(linkedToken);

            stopwatch.Stop();
            result.Duration = stopwatch.Elapsed;

            result.Success = (process.ExitCode == 0);
            result.Output = outputWriter.ToString();
            result.Error = errorWriter.ToString();

            await memoryMonitorTask;
        }
        catch (OperationCanceledException) when (cts.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
        {
            result.Error = "The process execution was terminated because it exceeded the maximum allowed execution time.";
        }
        catch (Exception ex)
        {
            result.Error = $"An error occurred: {ex.Message}";
        }
        finally
        {
            if (result.Duration == TimeSpan.Zero)
            {
                stopwatch.Stop();
                result.Duration = stopwatch.Elapsed;
            }
        }

        return result;
    }

    private static void MemoryMonitor(Process proc, long peakMemoryLimit, CancellationToken cancelToken)
    {
        while (!proc.HasExited)
        {
            if (proc.PeakPagedMemorySize64 > peakMemoryLimit)
            {
                proc.Kill();
                throw new InvalidOperationException("Process exceeded memory limit.");
            }

            cancelToken.ThrowIfCancellationRequested();
            Thread.Sleep(100);
        }
    }
}