using CodeSharp.Core.Executors.Models.Compilation;

namespace CodeSharp.Core.Executors;

public interface ICompileExecutor<TResponse>
{
    Task<TResponse> CompileAsync(string code, CompilationOptions? options = default, CancellationToken cancellationToken = default);
    Task<TResponse> CompileFileAsync(string filePath, CompilationOptions? options = default, CancellationToken cancellationToken = default);
}
