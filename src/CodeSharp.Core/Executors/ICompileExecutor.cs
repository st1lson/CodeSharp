using CodeSharp.Core.Executors.Models.Compilation;

namespace CodeSharp.Core.Executors;

public interface ICompileExecutor<TResponse> where TResponse : CompilationResponse
{
    Task<TResponse> CompileAsync(string code, bool run = false, CancellationToken cancellationToken = default);
    Task<TResponse> CompileFileAsync(string filePath, bool run = false, CancellationToken cancellationToken = default);
}
