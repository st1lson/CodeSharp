using CodeSharp.Executor.Constants;
using CodeSharp.Executor.Contracts.Testing;
using CodeSharp.Executor.Infrastructure.Interfaces;
using CodeSharp.Executor.Options;
using Microsoft.Extensions.Options;
using System.Xml.Linq;

namespace CodeSharp.Executor.Infrastructure.Parsers;

public class XmlTestReportParser : ITestReportParser
{
    private readonly ApplicationOptions _applicationOptions;

    public XmlTestReportParser(IOptions<ApplicationOptions> applicationOptions)
    {
        _applicationOptions = applicationOptions.Value;
    }

    public IList<TestResult> ParseTestReport()
    {
        var result = new List<TestResult>();

        try
        {
            var xmlFilePath = _applicationOptions.TestReportFilePath;
            var document = XDocument.Load(xmlFilePath);

            result = (
                from test in document.Descendants(XmlReportConstants.TestElementName)
                select new TestResult
                {
                    TestName = test.Attribute(XmlReportConstants.TestNameAttribute)!.Value,
                    Passed = test.Attribute(XmlReportConstants.PassedAttribute)!.Value == XmlReportConstants.TestPassedValue,
                    ExecutionTime = double.Parse(test.Attribute(XmlReportConstants.ExecutionTimeAttribute)!.Value),
                    ErrorMessage = ExtractErrorMessage(test.Element(XmlReportConstants.ErrorElementName))
                }
            ).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing XML report: {ex.Message}");
        }

        return result;
    }

    private static string? ExtractErrorMessage(XContainer? failureElement)
    {
        var messageElement = failureElement?.Element(XmlReportConstants.ErrorMessageElementName);

        return messageElement?.Value;
    }
}