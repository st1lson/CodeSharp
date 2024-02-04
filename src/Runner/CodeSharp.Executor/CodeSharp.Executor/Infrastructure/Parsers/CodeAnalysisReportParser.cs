using CodeSharp.Executor.Constants;
using CodeSharp.Executor.Infrastructure.Interfaces;
using CodeSharp.Executor.Options;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using CodeSharp.Executor.Contracts.Shared;

namespace CodeSharp.Executor.Infrastructure.Parsers;

public partial class CodeAnalysisReportParser : ICodeAnalysisReportParser
{
    private readonly ApplicationOptions _applicationOptions;

    public CodeAnalysisReportParser(IOptions<ApplicationOptions> applicationOptions)
    {
        _applicationOptions = applicationOptions.Value;
    }

    public async Task<CodeAnalysisReport> ParseCodeAnalysisReportAsync(CancellationToken cancellationToken)
    {
        var codeAnalysisResponse = new CodeAnalysisReport();

        var codeAnalysisFilePath = _applicationOptions.CodeAnalysisFilePath;
        var errorsFilePath = _applicationOptions.ErrorsFilePath;

        if (!File.Exists(codeAnalysisFilePath) || !File.Exists(errorsFilePath))
        {
            // TODO: Handle exception
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

            if (issue!.Severity == CodeAnalysisConstants.WarningKeyword)
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
            .Where(line => line.Contains(CodeAnalysisConstants.ErrorKeyword, StringComparison.OrdinalIgnoreCase) || line.Contains(CodeAnalysisConstants.WarningKeyword, StringComparison.OrdinalIgnoreCase));
    }

    private static bool TryExtractCodeAnalysisIssue(string codeAnalysisLine, out CodeAnalysisIssue? issue)
    {
        issue = default;
        
        var match = CodeAnalysisRegex().Match(codeAnalysisLine);
        if (!match.Success)
        {
            return false;
        }

        issue = new CodeAnalysisIssue
        {
            Line = int.Parse(match.Groups[CodeAnalysisConstants.RegexGroup.Line].Value),
            Column = int.Parse(match.Groups[CodeAnalysisConstants.RegexGroup.Column].Value),
            Severity = match.Groups[CodeAnalysisConstants.RegexGroup.ErrorType].Value,
            Code = match.Groups[CodeAnalysisConstants.RegexGroup.ErrorCode].Value,
            Message = match.Groups[CodeAnalysisConstants.RegexGroup.ErrorMessage].Value
        };

        return true;
    }

    [GeneratedRegex(".*\\((?<Line>\\d+),(?<Column>\\d+)\\): (?<ErrorType>\\w+) (?<ErrorCode>\\w+): (?<ErrorMessage>.+) \\[.*\\]")]
    private static partial Regex CodeAnalysisRegex();
}