using CodeSharp.Executor.Contracts;
using CodeSharp.Executor.Contracts.Internal;

namespace CodeSharp.Executor.Infrastructure.Interfaces;

public interface IProcessService
{
    Task<CompilationResponse> ExecuteProcessAsync(ProcessExecutionOptions executionOptions, CancellationToken cancellationToken = default);
}