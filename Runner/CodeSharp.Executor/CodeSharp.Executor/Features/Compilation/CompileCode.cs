using Carter;
using CodeSharp.Executor.Contracts.Compilation;
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
    }

    public sealed class Handler : IRequestHandler<Command, CompilationResponse>
    {
        private readonly ApplicationOptions _applicationOptions;
        private readonly IFileService _fileService;
        private readonly ICompilationService _compilationService;

        public Handler(IOptions<ApplicationOptions> applicationOptions, IFileService fileService, ICompilationService compilationService)
        {
            _fileService = fileService;
            _compilationService = compilationService;
            _applicationOptions = applicationOptions.Value;
        }

        public async Task<CompilationResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            await _fileService.ReplaceProgramFileAsync(request.Code, cancellationToken);

            return await _compilationService.CompileExecutableAsync(cancellationToken);
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