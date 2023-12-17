namespace CodeSharp.Core.Docker.Exceptions;

public class HealthcheckFailedException : Exception
{
    public HealthcheckFailedException(string message) : base(message)
    { }
}