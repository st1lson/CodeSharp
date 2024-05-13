using CodeSharp.Core.Contracts;
using CodeSharp.Core.Executors.Models.Compilation;

namespace CodeSharp.Core.Models;

public class CompilationLog : CompilationLog<Guid>
{
}

public class CompilationLog<TKey> : CompilationResponse, ICompilationLog<TKey>
{
    public required TKey Id { get; set; }

    public bool Success => ExecutedSuccessfully ?? CompiledSuccessfully;
}