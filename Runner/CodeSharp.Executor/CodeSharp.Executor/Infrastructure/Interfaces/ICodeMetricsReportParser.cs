using CodeSharp.Executor.Contracts.Internal;

namespace CodeSharp.Executor.Infrastructure.Interfaces;

public interface ICodeMetricsReportParser
{
    CodeMetricsReport Parse();
}