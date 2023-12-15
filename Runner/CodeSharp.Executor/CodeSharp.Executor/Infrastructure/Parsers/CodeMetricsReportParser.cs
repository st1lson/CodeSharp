using System.Xml.Linq;
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
    public string FileName { get; set; }
    public int LineNumber { get; set; }
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
        var document = XDocument.Load(@"M:\study\CodeSharp\Runner\CodeSharp.Executor\CodeSharp.Executor\res.xml");

        CodeMetrics codeMetrics = ParseMetricsNode(document.Descendants("Assembly").FirstOrDefault());

        return new CodeMetricsReport();
    }

    static CodeMetrics ParseMetricsNode(XElement element)
    {
        var metrics = new CodeMetrics();

        // Extract the metrics directly associated with this element
        foreach (var metric in element.Element("Metrics")?.Elements("Metric"))
        {
            switch (metric.Attribute("Name")?.Value)
            {
                case "MaintainabilityIndex":
                    metrics.MaintainabilityIndex = (int)metric.Attribute("Value");
                    break;
                case "CyclomaticComplexity":
                    metrics.CyclomaticComplexity = (int)metric.Attribute("Value");
                    break;
                case "ClassCoupling":
                    metrics.ClassCoupling = (int)metric.Attribute("Value");
                    break;
                case "DepthOfInheritance":
                    metrics.DepthOfInheritance = (int)metric.Attribute("Value");
                    break;
                case "SourceLines":
                    metrics.SourceLines = (int)metric.Attribute("Value");
                    break;
                case "ExecutableLines":
                    metrics.ExecutableLines = (int)metric.Attribute("Value");
                    break;
            }
        }

        if (element.Name.LocalName == "Method" ||
            element.Name.LocalName == "Field" ||
            element.Name.LocalName == "Property" ||
            element.Name.LocalName == "Accessor")
        {
            metrics.SourceInfo = new SourceInfo
            {
                FileName = element.Attribute("File")?.Value,
                LineNumber = (int?)element.Attribute("Line") ?? 0
            };
        }

        var namespaces = element.Elements("Namespaces").Elements("Namespace");
        foreach (var ns in namespaces)
        {
            metrics.NestedMetrics.Add(ParseMetricsNode(ns));
        }

        var types = element.Elements("Types").Elements("NamedType");
        foreach (var type in types)
        {
            metrics.NestedMetrics.Add(ParseMetricsNode(type));
        }

        var members = element.Elements("Members").Elements();
        foreach (var member in members)
        {
            metrics.NestedMetrics.Add(ParseMetricsNode(member));
        }

        return metrics;
    }
}