namespace CodeSharp.Core.Services.Exceptions;

public class TestingFailedException : Exception
{
    public TestingFailedException() : base("Testing failed")
    {
    }
}