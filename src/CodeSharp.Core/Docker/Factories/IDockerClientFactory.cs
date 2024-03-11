using Docker.DotNet;

namespace CodeSharp.Core.Docker.Factories;

public interface IDockerClientFactory
{
    IDockerClient CreateClient();
}
