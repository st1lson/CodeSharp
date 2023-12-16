using CodeSharp.Executor.Contracts;
using CodeSharp.Executor.Contracts.Testing;

namespace CodeSharp.Executor.Infrastructure.Interfaces;

public interface ITestReportParser
{
    TestingResponse ParseTestReport();
}