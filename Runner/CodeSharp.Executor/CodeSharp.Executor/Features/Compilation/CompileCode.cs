using Carter;
using CodeSharp.Executor.Contracts.Compilation;
using CodeSharp.Executor.Infrastructure.Interfaces;
using MediatR;

namespace CodeSharp.Executor.Features.Compilation;

public static class CompileCode
{
    public record Command : IRequest<CompilationResponse>
    {
        public required string Code { get; init; }
    }

    public sealed class Handler : IRequestHandler<Command, CompilationResponse>
    {
        private readonly IFileService _fileService;
        private readonly ICompilationService _compilationService;
        private readonly ICodeAnalysisService _codeAnalysisService;

        public Handler(
            IFileService fileService,
            ICompilationService compilationService,
            ICodeAnalysisService codeAnalysisService)
        {
            _fileService = fileService;
            _compilationService = compilationService;
            _codeAnalysisService = codeAnalysisService;
        }

        public async Task<CompilationResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            await _fileService.ReplaceProgramFileAsync(request.Code, cancellationToken);

            var compilationResponse = await _compilationService.CompileExecutableAsync(cancellationToken);

            var analysisResponse = await _codeAnalysisService.AnalyzeAsync(cancellationToken);

            compilationResponse.AnalysisResponse = analysisResponse;

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
                Code = request.Code
            };

            var result = await sender.Send(command);

            return Results.Ok(result);
        });
    }
}