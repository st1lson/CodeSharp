using Core.Docker.Models;
using Core.Docker.Providers;

namespace Core.Docker;

public interface IDockerConfiguration
{
    Image Image { get; }
    IDockerContainerNameProvider ContainerNameProvider { get; }
    IDockerContainerPortProvider ContainerPortProvider { get; }
    IDockerContainerEndpointProvider ContainerEndpointProvider { get; }
}
