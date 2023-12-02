using Core.Docker.Models;
using Core.Docker.Providers;

namespace Core.Docker;

public class DockerConfiguration : IDockerConfiguration
{
    public Image Image { get; }
    public IContainerNameProvider ContainerNameProvider { get; }
    public IContainerPortProvider ContainerPortProvider { get; }
    public IContainerEndpointProvider ContainerEndpointProvider { get; }

    public DockerConfiguration(
        Image image,
        IContainerNameProvider containerNameProvider,
        IContainerPortProvider containerPortProvider,
        IContainerEndpointProvider containerEndpointProvider)
    {
        Image = image ?? throw new ArgumentNullException(nameof(image));
        ContainerNameProvider = containerNameProvider ?? throw new ArgumentNullException(nameof(containerNameProvider));
        ContainerPortProvider = containerPortProvider ?? throw new ArgumentNullException(nameof(containerPortProvider));
        ContainerEndpointProvider = containerEndpointProvider ?? throw new ArgumentNullException(nameof(containerEndpointProvider));
    }
}
