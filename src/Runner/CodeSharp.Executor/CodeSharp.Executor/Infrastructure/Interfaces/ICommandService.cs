namespace CodeSharp.Executor.Infrastructure.Interfaces;

public interface ICommandService
{
    string GetCompilationCommand(string projectPath);
    string GetTestCommand(string projectPath);
    string GetRunCommand(string projectPath);
}
