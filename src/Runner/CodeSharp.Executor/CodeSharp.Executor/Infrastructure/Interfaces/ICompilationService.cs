using CodeSharp.Executor.Contracts.Compilation;

namespace CodeSharp.Executor.Infrastructure.Interfaces;

public interface ICompilationService
{
    Task<CompilationResponse> CompileTestsAsync(CancellationToken cancellationToken = default);
    Task<CompilationResponse> CompileExecutableAsync(CancellationToken cancellationToken = default);
}