namespace Core.Docker.Exceptions;

public class HealthcheckFailedException : Exception
{
    public HealthcheckFailedException(string message) : base(message)
    { }

    public HealthcheckFailedException(string message, Exception innerException) : base(message, innerException)
    { }
}