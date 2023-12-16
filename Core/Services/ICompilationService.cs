using Core.Services.Models.Compilation;

namespace Core.Services;

public interface ICompilationService
{
    Task<CompilationResponse> CompileAsync(string code, CancellationToken cancellationToken = default);
    Task<CompilationResponse> CompileFileAsync(string filePath, CancellationToken cancellationToken = default);
}
