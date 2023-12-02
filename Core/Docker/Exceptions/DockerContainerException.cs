namespace Core.Docker.Exceptions;

public class DockerContainerException : Exception
{
    public DockerContainerException(string message) : base(message)
    { }
}