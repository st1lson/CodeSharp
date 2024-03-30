using CodeSharp.Executor.Common.Exceptions;
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

        var hasInputs = executionOptions.Inputs is not null;

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
            process.StartInfo.RedirectStandardInput = hasInputs;
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
            var memoryMonitorTask = Task.CompletedTask;
            if (executionOptions.MaxRamUsageInMB.HasValue)
            {
                memoryMonitorTask = Task.Run(() => MemoryMonitor(process, executionOptions.MaxRamUsageInMB.Value, linkedToken), cancellationToken);
            }

            if (hasInputs)
            {
                while (executionOptions.Inputs!.Count > 0)
                {
                    string input = executionOptions.Inputs.Dequeue();
                    process.StandardInput.WriteLine(input);
                }
            }

            await process.WaitForExitAsync(linkedToken);

            stopwatch.Stop();
            result.Duration = stopwatch.Elapsed;

            result.Success = (process.ExitCode == 0);
            result.Output = outputWriter.ToString();
            result.Error = errorWriter.ToString();

            await memoryMonitorTask;
        }
        catch (MemoryLimitExceededException)
        {
            result.Error = "The process execution was terminated because it exceeded the maximum allowed memory usage.";
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
        var peakMemoryLimitInBytes = peakMemoryLimit * 1024L * 1024L;

        while (!proc.HasExited)
        {
            if (proc.PeakPagedMemorySize64 > peakMemoryLimitInBytes)
            {
                proc.Kill();
                throw new MemoryLimitExceededException();
            }

            cancelToken.ThrowIfCancellationRequested();
            Thread.Sleep(100);
        }
    }
}