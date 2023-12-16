using System.Xml.Linq;
using CodeSharp.Executor.Constants;
using CodeSharp.Executor.Contracts.Shared;
using CodeSharp.Executor.Infrastructure.Interfaces;
using CodeSharp.Executor.Options;
using Microsoft.Extensions.Options;

namespace CodeSharp.Executor.Infrastructure.Parsers;

public class CodeMetricsReportParser : ICodeMetricsReportParser
{
    private readonly ApplicationOptions _applicationOptions;

    public CodeMetricsReportParser(IOptions<ApplicationOptions> applicationOptions)
    {
        _applicationOptions = applicationOptions.Value;
    }

    public CodeMetricsReport Parse()
    {
        var document = XDocument.Load(_applicationOptions.CodeMetricsFilePath);

        var assemblyNode = document.Descendants(CodeMetricsReportConstants.Elements.Assembly).FirstOrDefault();

        return assemblyNode is null ? new CodeMetricsReport() : ParseMetricsNode(assemblyNode);
    }

    private static CodeMetricsReport ParseMetricsNode(XElement element)
    {
        var metrics = new CodeMetricsReport();

        var metricNodes = element.Element(CodeMetricsReportConstants.Elements.Metrics)?.Elements(CodeMetricsReportConstants.Elements.Metric) ?? Enumerable.Empty<XElement>();
        
        foreach (var metric in metricNodes)
        {
            switch (metric.Attribute(CodeMetricsReportConstants.Attributes.MetricName)?.Value)
            {
                case CodeMetricsReportConstants.MaintainabilityIndex:
                    metrics.MaintainabilityIndex = int.Parse(metric.Attribute(CodeMetricsReportConstants.Attributes.MetricValue)!.Value);
                    break;
                case CodeMetricsReportConstants.CyclomaticComplexity:
                    metrics.CyclomaticComplexity = int.Parse(metric.Attribute(CodeMetricsReportConstants.Attributes.MetricValue)!.Value);
                    break;
                case CodeMetricsReportConstants.ClassCoupling:
                    metrics.ClassCoupling = int.Parse(metric.Attribute(CodeMetricsReportConstants.Attributes.MetricValue)!.Value);
                    break;
                case CodeMetricsReportConstants.DepthOfInheritance:
                    metrics.DepthOfInheritance = int.Parse(metric.Attribute(CodeMetricsReportConstants.Attributes.MetricValue)!.Value);
                    break;
                case CodeMetricsReportConstants.SourceLines:
                    metrics.SourceLines = int.Parse(metric.Attribute(CodeMetricsReportConstants.Attributes.MetricValue)!.Value);
                    break;
                case CodeMetricsReportConstants.ExecutableLines:
                    metrics.ExecutableLines = int.Parse(metric.Attribute(CodeMetricsReportConstants.Attributes.MetricValue)!.Value);
                    break;
            }
        }

        if (element.Name.LocalName == CodeMetricsReportConstants.LocalNames.Method ||
            element.Name.LocalName == CodeMetricsReportConstants.LocalNames.Field ||
            element.Name.LocalName == CodeMetricsReportConstants.LocalNames.Property ||
            element.Name.LocalName == CodeMetricsReportConstants.LocalNames.Accessor)
        {
            metrics.SourceInfo = new SourceInfo
            {
                FileName = element.Attribute(CodeMetricsReportConstants.Attributes.SourceInfoFile)!.Value,
                LineNumber = (int?)element.Attribute(CodeMetricsReportConstants.Attributes.SourceInfoLine) ?? 0
            };
        }

        var namespaces = element.Elements(CodeMetricsReportConstants.Elements.Namespaces).Elements(CodeMetricsReportConstants.Elements.Namespace);
        foreach (var ns in namespaces)
        {
            metrics.NestedMetrics.Add(ParseMetricsNode(ns));
        }

        var types = element.Elements(CodeMetricsReportConstants.Elements.Types).Elements(CodeMetricsReportConstants.Elements.NamedType);
        foreach (var type in types)
        {
            metrics.NestedMetrics.Add(ParseMetricsNode(type));
        }

        var members = element.Elements(CodeMetricsReportConstants.Elements.Members).Elements();
        foreach (var member in members)
        {
            metrics.NestedMetrics.Add(ParseMetricsNode(member));
        }

        return metrics;
    }
}