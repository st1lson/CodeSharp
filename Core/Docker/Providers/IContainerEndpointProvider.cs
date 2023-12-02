namespace Core.Docker.Providers;

public interface IContainerEndpointProvider
{
    string GetHealthCheckEndpoint();
}