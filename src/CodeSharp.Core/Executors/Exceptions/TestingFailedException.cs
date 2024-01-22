namespace CodeSharp.Core.Executors.Exceptions;

public class TestingFailedException : Exception
{
    public TestingFailedException() : base("Testing failed")
    {
    }
}