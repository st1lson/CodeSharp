using CodeSharp.Executor.Contracts.Shared;

namespace CodeSharp.Executor.Infrastructure.Interfaces;

public interface IProcessService
{
    Task<ProcessExecution> ExecuteProcessAsync(ProcessExecutionOptions executionOptions, CancellationToken cancellationToken = default);
}