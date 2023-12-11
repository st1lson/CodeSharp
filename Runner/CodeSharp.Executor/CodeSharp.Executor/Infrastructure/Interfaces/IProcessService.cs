using CodeSharp.Executor.Contracts;
using CodeSharp.Executor.Contracts.Internal;

namespace CodeSharp.Executor.Infrastructure.Interfaces;

public interface IProcessService
{
    Task<T> ExecuteProcessAsync<T>(ProcessExecutionOptions executionOptions, CancellationToken cancellationToken = default) where T : CompilationResponse, new();
}