namespace CodeSharp.Core.Docker.Exceptions;

public class HealthCheckFailedException : Exception
{
    public HealthCheckFailedException(string message) : base(message)
    { }
}