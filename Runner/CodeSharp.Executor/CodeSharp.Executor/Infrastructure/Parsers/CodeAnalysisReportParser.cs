using System.Text.RegularExpressions;
using CodeSharp.Executor.Contracts.Internal;
using CodeSharp.Executor.Infrastructure.Interfaces;
using CodeSharp.Executor.Options;
using Microsoft.Extensions.Options;

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
                
            codeAnalysisResponse.CodeAnalysisIssues.Add(issue!);
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
        
        var regex = new Regex(@"^(?<position>\d+:\d+)>(?<type>error|warning) (?<code>\w+): (?<message>.+?) \[(?<file>.+?)\]$");
        var match = regex.Match(codeAnalysisLine);
        if (!match.Success)
        {
            return false;
        }

        issue = new CodeAnalysisIssue
        {
            Position = match.Groups["position"].Value,
            Severity = match.Groups["type"].Value,
            Code = match.Groups["code"].Value,
            Message = match.Groups["message"].Value,
            FileName = match.Groups["file"].Value
        };

        return true;
    }
}