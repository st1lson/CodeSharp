using Docker.DotNet;

namespace CodeSharp.Core.Docker.Factories;

public class DockerClientFactory : IDockerClientFactory
{
    public IDockerClient CreateClient()
    {
        return new DockerClientConfiguration().CreateClient();
    }
}
