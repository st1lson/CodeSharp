using Carter;
using CodeSharp.Executor.Contracts.Shared;
using CodeSharp.Executor.Contracts.Testing;
using CodeSharp.Executor.Infrastructure.Interfaces;
using CodeSharp.Executor.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace CodeSharp.Executor.Features.Testing;

public static class TestCode
{
    public record Command(string CodeToTest, string TestsCode) : IRequest<TestingResponse>;

    public sealed class Handler : IRequestHandler<Command, TestingResponse>
    {
        private readonly ApplicationOptions _applicationOptions;
        private readonly IFileService _fileService;
        private readonly IProcessService _processService;
        private readonly ITestReportParser _reportParser;
        private readonly ICompilationService _compilationService;
        private readonly ICodeAnalysisService _codeAnalysisService;

        public Handler(
            IOptions<ApplicationOptions> applicationOptions,
            IFileService fileService,
            IProcessService processService,
            ITestReportParser reportParser,
            ICompilationService compilationService,
            ICodeAnalysisService codeAnalysisService)
        {
            _fileService = fileService;
            _processService = processService;
            _reportParser = reportParser;
            _compilationService = compilationService;
            _applicationOptions = applicationOptions.Value;
            _codeAnalysisService = codeAnalysisService;
        }

        public async Task<TestingResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            await _fileService.ReplaceCodeToTestFileAsync(request.CodeToTest, cancellationToken);

            await _fileService.ReplaceTestsFileAsync(request.TestsCode, cancellationToken);
            
            var compilationResponse = await _compilationService.CompileTestsAsync(cancellationToken);

            var analysisResponse = await _codeAnalysisService.AnalyzeAsync(cancellationToken);

            if (!compilationResponse.Success)
            {
                return new TestingResponse
                {
                    CodeReport = analysisResponse
                };
            }

            var executionOptions = new ProcessExecutionOptions("dotnet",
                $"test {_applicationOptions.TestProjectPath} --configuration {_applicationOptions.TestConfigFilePath} --logger \"xunit;LogFilePath={_applicationOptions.TestReportFilePath}\"");

            var res = await _processService.ExecuteProcessAsync(executionOptions, cancellationToken);
            // TODO: Handle execution result
            //if (!res.Success)
            //{
            //    throw new Exception($"Output: {res.Output}\nError: {res.Error}");
            //}

            var testingResponse = _reportParser.ParseTestReport();

            testingResponse.CodeReport = analysisResponse;

            return testingResponse;
        }
    }
}

public class TestCodeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/test", async (TestingRequest request, ISender sender) =>
        {
            var command = new TestCode.Command(request.CodeToTest, request.TestsCode);

            var result = await sender.Send(command);

            return Results.Ok(result);
        });
    }
}