using Carter;
using CodeSharp.Executor.Contracts.Compilation;
using CodeSharp.Executor.Contracts.Shared;
using CodeSharp.Executor.Infrastructure.Interfaces;
using CodeSharp.Executor.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace CodeSharp.Executor.Features.Compilation;

public static class CompileCode
{
    public record Command : IRequest<CompilationResponse>
    {
        public required string Code { get; init; }
        public bool Run { get; init; }
    }

    public sealed class Handler : IRequestHandler<Command, CompilationResponse>
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

        public async Task<CompilationResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            await _fileService.ReplaceProgramFileAsync(request.Code, cancellationToken);

            var compilationResponse = await _compilationService.CompileExecutableAsync(cancellationToken);

            var analysisResponse = await _codeAnalysisService.AnalyzeAsync(cancellationToken);

            compilationResponse.CodeReport = analysisResponse;

            if (!request.Run || !compilationResponse.Success)
            {
                return compilationResponse;
            }

            var runOptions = new ProcessExecutionOptions("dotnet", $"run --project {_applicationOptions.ConsoleProjectPath} --no-build");
            
            var runResponse = await _processService.ExecuteProcessAsync(runOptions, cancellationToken);
            // TODO: Handle execution result
            if (!runResponse.Success)
            {
                throw new Exception($"Output: {runResponse.Output}\nError: {runResponse.Error}");
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
            var command = new CompileCode.Command
            {
                Code = request.Code,
                Run = request.Run
            };

            var result = await sender.Send(command);

            return Results.Ok(result);
        });
    }
}