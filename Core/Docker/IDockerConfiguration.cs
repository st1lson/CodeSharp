using Core.Docker.Providers;
using Core.Models;

namespace Core.Docker;

public interface IDockerConfiguration
{
    Image Image { get; }
    IDockerContainerNameProvider ContainerNameProvider { get; }
    IDockerContainerPortProvider ContainerPortProvider { get; }
    IDockerContainerEndpointProvider DockerContainerEndpointProvider { get; }
}
