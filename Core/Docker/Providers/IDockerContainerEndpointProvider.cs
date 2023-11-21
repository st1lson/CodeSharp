namespace Core.Docker.Providers;

public interface IDockerContainerEndpointProvider
{
    string GetHealthCheckEndpoint();
}