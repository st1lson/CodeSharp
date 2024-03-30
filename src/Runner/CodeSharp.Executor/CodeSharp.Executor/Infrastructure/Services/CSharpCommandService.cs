using CodeSharp.Executor.Options;
using Microsoft.Extensions.Options;

namespace CodeSharp.Executor.Infrastructure.Services;

public class CSharpCommandService
{
    private readonly ApplicationOptions _applicationOptions;

    public CSharpCommandService(IOptions<ApplicationOptions> applicationOptions)
    {
        _applicationOptions = applicationOptions.Value;
    }

    public string CreateCompilationCommand(string projectPath)
    {
        return $"build {projectPath} -nologo -noconsolelogger -flp1:logfile={_applicationOptions.ErrorsFilePath};errorsonly -flp2:logfile={_applicationOptions.CodeAnalysisFilePath};warningsonly";
    }

    public string CreateTestCommand(string projectPath)
    {
        return $"test {projectPath} --configuration {_applicationOptions.TestConfigFilePath} --logger \"xunit;LogFilePath={_applicationOptions.TestReportFilePath}\"";
    }

    public string CreateRunCommand(string projectPath)
    {
        return $"run --project {projectPath} --no-build";
    }
}
