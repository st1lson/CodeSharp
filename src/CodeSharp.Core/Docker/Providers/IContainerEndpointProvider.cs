namespace CodeSharp.Core.Docker.Providers;

public interface IContainerEndpointProvider
{
    string GetHealthCheckEndpoint();
    string GetCompileEndpoint();
    string GetTestingEndpoint();
}