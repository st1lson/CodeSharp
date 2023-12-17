using CodeSharp.Core.Services.Models.Compilation;

namespace CodeSharp.Core.Services;

public interface ICompilationService
{
    Task<CompilationResponse> CompileAsync(string code, bool run = false, CancellationToken cancellationToken = default);
    Task<CompilationResponse> CompileFileAsync(string filePath, bool run = false, CancellationToken cancellationToken = default);
}
