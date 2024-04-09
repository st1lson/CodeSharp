using CodeSharp.Core.Executors.Models.Compilation;
using CodeSharp.Core.Models;
using CodeSharp.Core.Services;
using Microsoft.AspNetCore.Mvc;
using CompilationRequest = CodeSharp.Samples.WebAPI.Models.Requests.CompilationRequest;

namespace CodeSharp.Samples.WebAPI.Controllers;

[Route("api/[controller]")]
public class CompilationController : ControllerBase
{
    private readonly ICompilationService<CompilationLog, Guid> _compilationService;

    public CompilationController(ICompilationService<CompilationLog, Guid> compilationService)
    {
        _compilationService = compilationService;
    }

    [HttpPost]
    public async Task<IActionResult> CompileAsync([FromBody] CompilationRequest request)
    {
        var options = new CompilationOptions
        {
            Run = request.Run,
            MaxCompilationTime = TimeSpan.FromMicroseconds((double)request.MaxCompilationTime!),
            MaxExecutionTime = TimeSpan.FromMicroseconds((double)request.MaxExecutionTime!),
            MaxRamUsage = request.MaxRamUsage,
            Inputs = request.Inputs,
        };

        var compilationResult = await _compilationService.CompileAsync(request.Code, options);

        return Ok(compilationResult);
    }
}
