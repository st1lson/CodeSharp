﻿using Carter;
using CodeSharp.Executor.Contracts.Internal;
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

        public Handler(IOptions<ApplicationOptions> applicationOptions, IFileService fileService, IProcessService processService, ITestReportParser reportParser, ICompilationService compilationService)
        {
            _fileService = fileService;
            _processService = processService;
            _reportParser = reportParser;
            _compilationService = compilationService;
            _applicationOptions = applicationOptions.Value;
        }

        public async Task<TestingResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            await _fileService.ReplaceCodeToTestFileAsync(request.CodeToTest, cancellationToken);

            await _fileService.ReplaceTestsFileAsync(request.TestsCode, cancellationToken);

            var compilationResponse = await _compilationService.CompileTestsAsync(cancellationToken);

            var executionOptions = new ProcessExecutionOptions("dotnet",
                $"test {_applicationOptions.TestProjectPath} --configuration xunit.runner.json --logger \"xunit;LogFilePath={_applicationOptions.TestReportFilePath}\"");

            // TODO: Handle execution result
            var _ = await _processService.ExecuteProcessAsync(executionOptions, cancellationToken);

            var testingResponse = _reportParser.ParseTestReport();

            //TODO: Add mapper
            testingResponse.CodeAnalysisIssues = compilationResponse.CodeAnalysisIssues;
            testingResponse.CodeGrade = compilationResponse.CodeGrade;
            testingResponse.Errors = compilationResponse.Errors;

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