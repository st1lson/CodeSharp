using CodeSharp.Executor.Contracts.Internal;
using CodeSharp.Executor.Infrastructure.Interfaces;
using CodeSharp.Executor.Options;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace CodeSharp.Executor.Infrastructure.Parsers;

public class CodeAnalysisReportParser : ICodeAnalysisReportParser
{
    private readonly ApplicationOptions _applicationOptions;

    public CodeAnalysisReportParser(IOptions<ApplicationOptions> applicationOptions)
    {
        _applicationOptions = applicationOptions.Value;
    }

    public async Task<CodeAnalysisResponse> ParseCodeAnalysisReportAsync(CancellationToken cancellationToken)
    {
        var codeAnalysisResponse = new CodeAnalysisResponse();

        var codeAnalysisFilePath = _applicationOptions.CodeAnalysisFilePath;
        var errorsFilePath = _applicationOptions.ErrorsFilePath;

        if (!File.Exists(codeAnalysisFilePath) || !File.Exists(errorsFilePath))
        {
            throw new Exception();
        }

        string logContent = await File.ReadAllTextAsync(codeAnalysisFilePath, cancellationToken);
        string errorsContent = await File.ReadAllTextAsync(errorsFilePath, cancellationToken);

        var codeProblems = string.Concat(logContent, errorsContent);

        var codeAnalysisLines = ExtractCodeAnalysisLines(codeProblems);

        foreach (var line in codeAnalysisLines)
        {
            if (!TryExtractCodeAnalysisIssue(line, out var issue))
            {
                continue;
            }

            if (issue!.Severity == "warning")
            {
                codeAnalysisResponse.CodeAnalysisIssues.Add(issue!);
            }
            else
            {
                codeAnalysisResponse.Errors.Add(issue);
            }
        }

        return codeAnalysisResponse;
    }

    private static IEnumerable<string> ExtractCodeAnalysisLines(string logContent)
    {
        return logContent
            .Split('\n')
            .Where(line => line.Contains("warning", StringComparison.OrdinalIgnoreCase) || line.Contains("error", StringComparison.OrdinalIgnoreCase));
    }

    private static bool TryExtractCodeAnalysisIssue(string codeAnalysisLine, out CodeAnalysisIssue? issue)
    {
        issue = default;

        //TODO: review the regex to probably clean it up
        //string pattern = @"(\d+:\d+)>(.+)\((\d+,\d+)\): (\w+) (\w+): (.+) \[([^\]]+)\]";
        //string pattern = @"(?<Position>\d+:\d+)>(?<FilePath>.+)\((?<LineNumber>\d+,\d+)\): (?<ErrorType>\w+) (?<ErrorCode>\w+): (?<ErrorMessage>.+) \[(?<ProjectFilePath>[^\]]+)\]";
        string pattern = @"(?<Line>\d+):(?<Column>\d+)>.+: (?<ErrorType>\w+) (?<ErrorCode>\w+): (?<ErrorMessage>.+) \[.*\]";
        var regex = new Regex(pattern);
        var match = regex.Match(codeAnalysisLine);
        if (!match.Success)
        {
            return false;
        }

        issue = new CodeAnalysisIssue
        {
            Line = int.Parse(match.Groups["Line"].Value),
            Column = int.Parse(match.Groups["Column"].Value),
            Severity = match.Groups["ErrorType"].Value,
            Code = match.Groups["ErrorCode"].Value,
            Message = match.Groups["ErrorMessage"].Value
        };

        return true;
    }
}