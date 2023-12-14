namespace CodeSharp.Executor.Constants;

public static class CodeAnalysisConstants
{
    public const string ErrorKeyword = "error";
    public const string WarningKeyword = "warning";

    public static class RegexGroup
    {
        public const string Line = "Line";
        public const string Column = "Column";
        public const string ErrorType = "ErrorType";
        public const string ErrorCode = "ErrorCode";
        public const string ErrorMessage = "ErrorMessage";
    }
}
