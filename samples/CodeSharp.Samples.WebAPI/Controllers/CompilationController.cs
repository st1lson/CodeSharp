using CodeSharp.Core.Models;
using CodeSharp.Core.Services;
using CodeSharp.Samples.WebAPI.Models.Requests;
using Microsoft.AspNetCore.Mvc;

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
        var compilationResult = await _compilationService.CompileAsync(request.Code, true);

        return Ok(compilationResult);
    }
}
