using CodeSharp.Executor.Contracts.Internal;
using CodeSharp.Executor.Infrastructure.Interfaces;
using CodeSharp.Executor.Options;
using Microsoft.Extensions.Options;
using System.Xml.Linq;

namespace CodeSharp.Executor.Infrastructure.Parsers;

public class CodeMetrics
{
    public int MaintainabilityIndex { get; set; }
    public int CyclomaticComplexity { get; set; }
    public int ClassCoupling { get; set; }
    public int DepthOfInheritance { get; set; }
    public int SourceLines { get; set; }
    public int ExecutableLines { get; set; }

    public List<CodeMetrics> NestedMetrics { get; set; } = new List<CodeMetrics>();
    public SourceInfo SourceInfo { get; set; }
}

public class SourceInfo
{
    public string File { get; set; }
    public int Line { get; set; }
    public string Method { get; set; }
}

public class CodeMetricsReportParser : ICodeMetricsReportParser
{
    private readonly ApplicationOptions _applicationOptions;

    public CodeMetricsReportParser(IOptions<ApplicationOptions> applicationOptions)
    {
        _applicationOptions = applicationOptions.Value;
    }

    public CodeMetricsReport Parse()
    {
        var document = XDocument.Load(@"C:\Users\st1lson\Desktop\repos\CodeSharp\Runner\CodeSharp.Executor\CodeSharp.Executor\res.xml");

        CodeMetrics codeMetrics = ParseMetrics(document.Root.Element("Targets").Element("Target").Element("Assembly"));

        return new CodeMetricsReport
        {
            ClassCoupling = codeMetrics.ClassCoupling,
            CyclomaticComplexity = codeMetrics.CyclomaticComplexity,
            MaintainabilityIndex = codeMetrics.MaintainabilityIndex,
            SourceLines = codeMetrics.SourceLines,
            ExecutableLines = codeMetrics.ExecutableLines,
            DepthOfInheritance = codeMetrics.DepthOfInheritance,
        };
    }

    static CodeMetrics ParseMetrics(XElement element)
    {
        XElement metricsElement = element.Element("Metrics");

        CodeMetrics metrics = new CodeMetrics();
        if (metricsElement != null)
        {
            foreach (XElement metricElement in metricsElement.Elements("Metric"))
            {
                string metricName = metricElement.Attribute("Name")?.Value;
                string metricValue = metricElement.Attribute("Value")?.Value;

                if (!string.IsNullOrEmpty(metricName) && !string.IsNullOrEmpty(metricValue))
                {
                    switch (metricName)
                    {
                        case "MaintainabilityIndex":
                            metrics.MaintainabilityIndex = Convert.ToInt32(metricValue);
                            break;
                        case "CyclomaticComplexity":
                            metrics.CyclomaticComplexity = Convert.ToInt32(metricValue);
                            break;
                        case "ClassCoupling":
                            metrics.ClassCoupling = Convert.ToInt32(metricValue);
                            break;
                        case "DepthOfInheritance":
                            metrics.DepthOfInheritance = Convert.ToInt32(metricValue);
                            break;
                        case "SourceLines":
                            metrics.SourceLines = Convert.ToInt32(metricValue);
                            break;
                        case "ExecutableLines":
                            metrics.ExecutableLines = Convert.ToInt32(metricValue);
                            break;
                    }
                }
            }

            metrics.NestedMetrics.AddRange(metricsElement.Elements("Metrics").Select(ParseMetrics));
        }

        metrics.SourceInfo = ParseSourceInfo(element);

        return metrics;
    }

    static SourceInfo ParseSourceInfo(XElement element)
    {
        XElement sourceInfoElement = element.Elements("Members").Elements("Method").Elements("Metrics").FirstOrDefault();

        if (sourceInfoElement != null)
        {
            return new SourceInfo
            {
                File = sourceInfoElement.Attribute("File")?.Value ?? string.Empty,
                Line = Convert.ToInt32(sourceInfoElement.Attribute("Line")?.Value ?? "0"),
                Method = sourceInfoElement.Attribute("Name")?.Value ?? string.Empty
            };
        }

        return new SourceInfo();
    }
}