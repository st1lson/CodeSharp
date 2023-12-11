using CodeSharp.Executor.Contracts;

namespace CodeSharp.Executor.Infrastructure.Interfaces;

public interface ITestReportParser
{
    TestingResponse ParseTestReport();
}