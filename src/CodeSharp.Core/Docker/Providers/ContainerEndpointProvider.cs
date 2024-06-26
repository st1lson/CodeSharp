﻿namespace CodeSharp.Core.Docker.Providers;

public class ContainerEndpointProvider : IContainerEndpointProvider
{
    private readonly IContainerPortProvider _portProvider;

    public ContainerEndpointProvider(IContainerPortProvider portProvider)
    {
        _portProvider = portProvider;
    }

    public string GetCompileEndpoint()
    {
        return $"http://localhost:{_portProvider.CurrentPort}/api/compile";
    }

    public string GetHealthCheckEndpoint()
    {
        return $"http://localhost:{_portProvider.CurrentPort}/healthz";
    }

    public string GetTestingEndpoint()
    {
        return $"http://localhost:{_portProvider.CurrentPort}/api/test";
    }
}