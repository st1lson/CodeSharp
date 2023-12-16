﻿using CodeSharp.Samples.WebAPI.Models.Requests;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeSharp.Samples.WebAPI.Controllers;

[Route("api/[controller]")]
public class CompilationController : ControllerBase
{
    private readonly ICompilationService _compilationService;

    public CompilationController(ICompilationService compilationService)
    {
        _compilationService = compilationService;
    }

    [HttpPost]
    public async Task<IActionResult> CompileAsync([FromBody] CompilationRequest request)
    {
        var compilationResult = await _compilationService.CompileAsync(request.Code);

        return Ok(compilationResult);
    }
}
