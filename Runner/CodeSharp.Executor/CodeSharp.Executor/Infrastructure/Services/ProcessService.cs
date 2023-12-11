using System.Diagnostics;
using CodeSharp.Executor.Contracts;
using CodeSharp.Executor.Contracts.Internal;
using CodeSharp.Executor.Infrastructure.Interfaces;

namespace CodeSharp.Executor.Infrastructure.Services;

public class ProcessService : IProcessService
{
    public async Task<T> ExecuteProcessAsync<T>(
        ProcessExecutionOptions executionOptions,
        CancellationToken cancellationToken = default) where T : CompilationResponse, new()
    {
        var result = new T();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        try
        {
            using Process process = new Process();
            process.StartInfo.FileName = executionOptions.FileName;
            process.StartInfo.Arguments = executionOptions.Arguments;
            process.StartInfo.RedirectStandardOutput = executionOptions.RedirectStandardOutput;
            process.StartInfo.RedirectStandardError = executionOptions.RedirectStandardError;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = executionOptions.CreateNoWindow;

            StringWriter outputWriter = new StringWriter();
            StringWriter errorWriter = new StringWriter();

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

            int exitCode = process.ExitCode;

            result.Success = (exitCode == 0);
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