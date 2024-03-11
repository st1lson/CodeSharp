using CodeSharp.Core.Docker.Factories;
using Docker.DotNet;
using Xunit;

namespace CodeSharp.Core.Tests.Docker.Factories;

public class DockerClientFactoryTests
{
    private readonly IDockerClientFactory _dockerClientFactory;

    public DockerClientFactoryTests()
    {
        _dockerClientFactory = new DockerClientFactory();
    }

    [Fact]
    public void CreateClient_ReturnsNonNullClient()
    {
        // Act
        var client = _dockerClientFactory.CreateClient();

        // Assert
        Assert.NotNull(client);
        Assert.IsType<DockerClient>(client);
    }

    [Fact]
    public void CreateClient_ReturnsDifferentInstances()
    {
        // Act
        var client1 = _dockerClientFactory.CreateClient();
        var client2 = _dockerClientFactory.CreateClient();

        // Assert
        Assert.NotSame(client1, client2);
    }
}
