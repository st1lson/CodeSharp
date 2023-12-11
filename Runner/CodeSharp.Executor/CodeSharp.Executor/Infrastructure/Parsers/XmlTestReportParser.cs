using System.Xml.Linq;
using CodeSharp.Executor.Constants;
using CodeSharp.Executor.Contracts;
using CodeSharp.Executor.Infrastructure.Interfaces;
using CodeSharp.Executor.Options;
using Microsoft.Extensions.Options;

namespace CodeSharp.Executor.Infrastructure.Parsers;

public class XmlTestReportParser : ITestReportParser
{
    private readonly ApplicationOptions _applicationOptions;

    public XmlTestReportParser(IOptions<ApplicationOptions> applicationOptions)
    {
        _applicationOptions = applicationOptions.Value;
    }

    public TestingResponse ParseTestReport()
    {
        var result = new TestingResponse();

        try
        {
            var xmlFilePath = _applicationOptions.TestReportFilePath;
            var document = XDocument.Load(xmlFilePath);

            result.TestResults = (
                from test in document.Descendants(XmlReportConstants.TestElementName)
                select new TestResult
                {
                    TestName = (string)test.Attribute(XmlReportConstants.TestNameAttribute),
                    Passed = (string)test.Attribute(XmlReportConstants.PassedAttribute) == XmlReportConstants.TestPassedValue,
                    ExecutionTime = (double)test.Attribute(XmlReportConstants.ExecutionTimeAttribute),
                    ErrorMessage = ExtractErrorMessage(test.Element(XmlReportConstants.ErrorElementName))
                }
            ).ToList();
        }
        catch (Exception ex)
        {
            //TODO: Add result object to return errors
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