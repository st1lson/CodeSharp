using Carter;
using CodeSharp.Executor.Common.Extensions;
using CodeSharp.Executor.Contracts.Compilation;
using CodeSharp.Executor.Contracts.Shared;
using CodeSharp.Executor.Infrastructure.Interfaces;
using CodeSharp.Executor.Options;
using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Error = ErrorOr.Error;

namespace CodeSharp.Executor.Features.Compilation;

public static class CompileCode
{
    public record Command(string Code, CompilationOptions Options) : IRequest<ErrorOr<CompilationResponse>>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Code)
                .NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<Command, ErrorOr<CompilationResponse>>
    {
        private readonly IFileService _fileService;
        private readonly IProcessService _processService;
        private readonly ICompilationService _compilationService;
        private readonly ICodeAnalysisService _codeAnalysisService;
        private readonly ApplicationOptions _applicationOptions;

        public Handler(
            IFileService fileService,
            ICompilationService compilationService,
            ICodeAnalysisService codeAnalysisService,
            IProcessService processService,
            IOptions<ApplicationOptions> applicationOptions)
        {
            _fileService = fileService;
            _compilationService = compilationService;
            _codeAnalysisService = codeAnalysisService;
            _processService = processService;
            _applicationOptions = applicationOptions.Value;
        }

        public async Task<ErrorOr<CompilationResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            await _fileService.ReplaceProgramFileAsync(request.Code, cancellationToken);

            var compilationResult = await _compilationService.CompileExecutableAsync(request.Options.MaxCompilationTime, request.Options.MaxRamUsage, cancellationToken);

            var analysisResponse = await _codeAnalysisService.AnalyzeAsync(cancellationToken);

            if (!compilationResult.Success)
            {
                analysisResponse.Errors.Add(new CodeAnalysisIssue { Message = compilationResult.Error! });
            }

            var compilationResponse = new CompilationResponse
            {
                Success = compilationResult.Success,
                Duration = compilationResult.Duration,
                CodeReport = analysisResponse
            };

            if (!request.Options.Run || !compilationResponse.Success)
            {
                return compilationResponse;
            }

            var runOptions = new ProcessExecutionOptions("dotnet", $"run --project {_applicationOptions.ConsoleProjectPath} --no-build");

            var runResponse = await _processService.ExecuteProcessAsync(runOptions, cancellationToken);
            if (!runResponse.Success)
            {
                return Error.Failure($"Output: {runResponse.Output}\n Stack: {runResponse.Error}");
            }

            compilationResponse.Output = runResponse.Output;

            return compilationResponse;
        }
    }
}

public class CompileCodeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/compile", async (CompilationRequest request, ISender sender) =>
        {
            var command = new CompileCode.Command(request.Code, request.Options);

            var result = await sender.Send(command);

            return Results.Extensions.ErrorOrResult(result);
        });
    }
}