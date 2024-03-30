using CodeSharp.Executor.Contracts.Shared;

namespace CodeSharp.Executor.Infrastructure.Interfaces;

public interface ICompilationService
{
    Task<ProcessExecution> CompileTestsAsync(TimeSpan? maxDuration = default, long? maxRamUsage = default, CancellationToken cancellationToken = default);
    Task<ProcessExecution> CompileExecutableAsync(TimeSpan? maxDuration = default, long? maxRamUsage = default, CancellationToken cancellationToken = default);
}