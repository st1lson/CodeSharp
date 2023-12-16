using CodeSharp.Executor.Contracts.Shared;

namespace CodeSharp.Executor.Infrastructure.Interfaces;

public interface ICodeMetricsReportParser
{
    CodeMetricsReport Parse();
}