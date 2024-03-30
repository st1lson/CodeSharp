using CodeSharp.Executor.Contracts.Shared;
using CodeSharp.Executor.Infrastructure.Interfaces;
using CodeSharp.Executor.Options;
using Microsoft.Extensions.Options;

namespace CodeSharp.Executor.Infrastructure.Services;

public class CompilationService : ICompilationService
{
    private readonly IProcessService _processService;
    private readonly ApplicationOptions _applicationOptions;

    public CompilationService(
        IOptions<ApplicationOptions> applicationOptions,
        IProcessService processService)
    {
        _processService = processService;
        _applicationOptions = applicationOptions.Value;
    }

    public Task<ProcessExecution> CompileTestsAsync(TimeSpan? maxDuration = default, long? maxRamUsage = default, CancellationToken cancellationToken = default)
    {
        return CompileAsync(_applicationOptions.TestProjectPath, maxDuration, maxRamUsage, cancellationToken);
    }

    public Task<ProcessExecution> CompileExecutableAsync(TimeSpan? maxDuration = default, long? maxRamUsage = default, CancellationToken cancellationToken = default)
    {
        return CompileAsync(_applicationOptions.ConsoleProjectPath, maxDuration, maxRamUsage, cancellationToken);
    }

    private Task<ProcessExecution> CompileAsync(string projectPath, TimeSpan? maxDuration = default, long? maxRamUsage = default, CancellationToken cancellationToken = default)
    {
        var executionOptions = new ProcessExecutionOptions(
            "dotnet",
            $"build {projectPath} -nologo -noconsolelogger -flp1:logfile={_applicationOptions.ErrorsFilePath};errorsonly -flp2:logfile={_applicationOptions.CodeAnalysisFilePath};warningsonly",
            MaxDuration: maxDuration,
            MaxRamUsageInMB: maxRamUsage);

        return _processService.ExecuteProcessAsync(executionOptions, cancellationToken);
    }
}