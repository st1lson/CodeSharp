namespace CodeSharp.Executor.Constants;

public static class CodeMetricsReportConstants
{
    public const string MaintainabilityIndex = "MaintainabilityIndex";
    public const string CyclomaticComplexity = "CyclomaticComplexity";
    public const string ClassCoupling = "ClassCoupling";
    public const string DepthOfInheritance = "DepthOfInheritance";
    public const string SourceLines = "SourceLines";
    public const string ExecutableLines = "ExecutableLines";

    public static class Elements
    {
        public const string Assembly = "Assembly";
        public const string Metrics = "Metrics";
        public const string Metric = "Metric";
        public const string Namespaces = "Namespaces";
        public const string Namespace = "Namespace";
        public const string Types = "Types";
        public const string NamedType = "NamedType";
        public const string Members = "Members";
    }

    public static class LocalNames
    {
        public const string Method = "Method";
        public const string Field = "Field";
        public const string Property = "Property";
        public const string Accessor = "Accessor";
    }

    public static class Attributes
    {
        public const string MetricName = "Name";
        public const string MetricValue = "Value";

        public const string SourceInfoFile = "File";
        public const string SourceInfoLine = "Line";
    }
}