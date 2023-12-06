namespace Core.Docker.Providers;

public class ContainerEndpointProvider : IContainerEndpointProvider
{
    private readonly IContainerPortProvider _portProvider;

    public ContainerEndpointProvider(IContainerPortProvider portProvider)
    {
        _portProvider = portProvider;
    }

    public string GetCompileEndpoint()
    {
        return $"http://localhost:{_portProvider.CurrentPort}/compiler";
    }

    public string GetHealthCheckEndpoint()
    {
        return $"http://localhost:{_portProvider.CurrentPort}/health";
    }
}