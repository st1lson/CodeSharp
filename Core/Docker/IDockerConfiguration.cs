using Core.Docker.Models;
using Core.Docker.Providers;

namespace Core.Docker;

public interface IDockerConfiguration
{
    Image Image { get; }
    IContainerNameProvider ContainerNameProvider { get; }
    IContainerPortProvider ContainerPortProvider { get; }
    IContainerEndpointProvider ContainerEndpointProvider { get; }
}
