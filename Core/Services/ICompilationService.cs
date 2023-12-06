﻿using Core.Services.Models;

namespace Core.Services;

public interface ICompilationService
{
    Task<CompilationResult> CompileAsync(string code, CancellationToken cancellationToken = default);
}