using Carter;
using CodeSharp.Executor.Common.Extensions;
using CodeSharp.Executor.Constants;
using CodeSharp.Executor.Contracts.Compilation;
using CodeSharp.Executor.Contracts.Shared;
using CodeSharp.Executor.Contracts.Testing;
using CodeSharp.Executor.Infrastructure.Interfaces;
using CodeSharp.Executor.Options;
using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;

namespace CodeSharp.Executor.Features.Testing;

public static class TestCode
{
    public record Command(string CodeToTest, string TestsCode, TestingOptions Options) : IRequest<ErrorOr<TestingResponse>>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.CodeToTest)
                .NotEmpty();

            RuleFor(c => c.TestsCode)
                .NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<Command, ErrorOr<TestingResponse>>
    {
        private readonly ApplicationOptions _applicationOptions;
        private readonly IFileService _fileService;
        private readonly IProcessService _processService;
        private readonly ITestReportParser _reportParser;
        private readonly ICommandService _commandService;
        private readonly ICodeAnalysisService _codeAnalysisService;

        public Handler(
            IOptions<ApplicationOptions> applicationOptions,
            IFileService fileService,
            IProcessService processService,
            ITestReportParser reportParser,
            ICommandService commandService,
            ICodeAnalysisService codeAnalysisService)
        {
            _fileService = fileService;
            _processService = processService;
            _reportParser = reportParser;
            _commandService = commandService;
            _applicationOptions = applicationOptions.Value;
            _codeAnalysisService = codeAnalysisService;
        }

        public async Task<ErrorOr<TestingResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var (codeToTest, testsCode, options) = request;

            await _fileService.ReplaceCodeToTestFileAsync(codeToTest, cancellationToken);

            await _fileService.ReplaceTestsFileAsync(testsCode, cancellationToken);


            var compilationOptions = new ProcessExecutionOptions(
                ExecutionConstants.ExecutorName,
                _commandService.GetCompilationCommand(_applicationOptions.TestProjectPath),
                MaxDuration: options.MaxCompilationTime,
                MaxRamUsageInMB: options.MaxRamUsage);

            var compilationResult = await _processService.ExecuteProcessAsync(compilationOptions, cancellationToken);

            var analysisResponse = await _codeAnalysisService.AnalyzeAsync(cancellationToken);

            if (!compilationResult.Success)
            {
                AppendError(compilationResult.Error!);
            }

            var compilationResponse = new CompilationResponse
            {
                Success = compilationResult.Success,
                Duration = compilationResult.Duration,
                CodeReport = analysisResponse
            };

            if (!compilationResponse.Success)
            {
                return new TestingResponse
                {
                    CodeReport = analysisResponse
                };
            }

            var testingOptions = new ProcessExecutionOptions(
                ExecutionConstants.ExecutorName,
                _commandService.GetTestCommand(_applicationOptions.TestProjectPath),
                MaxDuration: options.MaxTestingTime,
                MaxRamUsageInMB: options.MaxRamUsage);

            var testingResult = await _processService.ExecuteProcessAsync(testingOptions, cancellationToken);
            if (!testingResult.Success)
            {
                AppendError(testingResult.Error!);
            }

            var testingResponse = _reportParser.ParseTestReport();

            testingResponse.CodeReport = analysisResponse;

            return testingResponse;

            void AppendError(string message)
            {
                if (analysisResponse is null || string.IsNullOrEmpty(message))
                {
                    return;
                }

                analysisResponse.Errors.Add(new CodeAnalysisIssue { Message = message });
            }
        }
    }
}

public class TestCodeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/test", async (TestingRequest request, ISender sender) =>
        {
            var command = new TestCode.Command(request.CodeToTest, request.TestsCode, request.Options);

            var result = await sender.Send(command);

            return Results.Extensions.ErrorOrResult(result);
        });
    }
}