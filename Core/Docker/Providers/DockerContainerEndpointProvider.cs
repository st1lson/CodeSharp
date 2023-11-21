namespace Core.Docker.Providers;

public class DockerContainerEndpointProvider : IDockerContainerEndpointProvider
{
    private readonly IDockerContainerPortProvider _portProvider;

    public DockerContainerEndpointProvider(IDockerContainerPortProvider portProvider)
    {
        _portProvider = portProvider;
    }

    public string GetHealthCheckEndpoint()
    {
        return $"http://localhost:{_portProvider.CurrentPort}/health";
    }
}