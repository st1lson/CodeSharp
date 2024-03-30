using CodeSharp.Executor.Contracts.Testing;

namespace CodeSharp.Executor.Infrastructure.Interfaces;

public interface ITestReportParser
{
    IList<TestResult> ParseTestReport();
}