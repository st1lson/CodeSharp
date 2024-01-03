namespace CodeSharp.Core.Services.Exceptions;

public class CompilationFailedException : Exception
{
    public CompilationFailedException() : base("Compilation failed")
    {
    }
}
