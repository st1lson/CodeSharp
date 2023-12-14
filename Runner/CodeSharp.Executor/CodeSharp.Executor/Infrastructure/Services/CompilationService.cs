using CodeSharp.Executor.Contracts.Compilation;
using CodeSharp.Executor.Contracts.Internal;
using CodeSharp.Executor.Infrastructure.Interfaces;
using CodeSharp.Executor.Options;
using Microsoft.Extensions.Options;

namespace CodeSharp.Executor.Infrastructure.Services;

public class CompilationService : ICompilationService
{
    private readonly IProcessService _processService;
    private readonly ApplicationOptions _applicationOptions;
    private readonly ICodeAnalysisReportParser _codeAnalysisReportParser;

    public CompilationService(
        IOptions<ApplicationOptions> applicationOptions,
        IProcessService processService,
        ICodeAnalysisReportParser codeAnalysisReportParser)
    {
        _processService = processService;
        _codeAnalysisReportParser = codeAnalysisReportParser;
        _applicationOptions = applicationOptions.Value;
    }

    public Task<CompilationResponse> CompileTestsAsync(CancellationToken cancellationToken)
    {
        return CompileAsync(_applicationOptions.TestProjectPath, cancellationToken);
    }

    public Task<CompilationResponse> CompileExecutableAsync(CancellationToken cancellationToken)
    {
        return CompileAsync(_applicationOptions.ConsoleProjectPath, cancellationToken);
    }

    private async Task<CompilationResponse> CompileAsync(string projectPath, CancellationToken cancellationToken)
    {
        var executionOptions = new ProcessExecutionOptions("dotnet", $"build {projectPath} -nologo -noconsolelogger -filelogger -flp1:logfile={_applicationOptions.ErrorsFilePath};errorsonly -flp2:logfile={_applicationOptions.CodeAnalysisFilePath};append;warningsonly");

        var compilationResponse = await _processService.ExecuteProcessAsync<CompilationResponse>(executionOptions, cancellationToken);
        
        var codeAnalysisResponse = await _codeAnalysisReportParser.ParseCodeAnalysisReportAsync(cancellationToken);

        return compilationResponse;
    }
}