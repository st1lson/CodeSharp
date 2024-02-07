using CodeSharp.Core.Docker.Providers;
using NSubstitute;
using Xunit;

namespace CodeSharp.Core.Tests.Docker.Providers;

public sealed class ContainerEndpointProviderTests
{
    private readonly IContainerPortProvider _portProvider;

    public ContainerEndpointProviderTests()
    {
        _portProvider = Substitute.For<IContainerPortProvider>();
    }

    [Fact]
    public void GetCompileEndpoint_ReturnsCorrectEndpoint()
    {
        // Arrange
        int expectedPort = 1234;
        _portProvider.CurrentPort.Returns(expectedPort);
        var endpointProvider = new ContainerEndpointProvider(_portProvider);

        // Act
        string compileEndpoint = endpointProvider.GetCompileEndpoint();

        // Assert
        string expectedEndpoint = $"http://localhost:{expectedPort}/api/compile";
        Assert.Equal(expectedEndpoint, compileEndpoint);
    }

    [Fact]
    public void GetHealthCheckEndpoint_ReturnsCorrectEndpoint()
    {
        // Arrange
        int expectedPort = 5678;
        _portProvider.CurrentPort.Returns(expectedPort);
        var endpointProvider = new ContainerEndpointProvider(_portProvider);

        // Act
        string healthCheckEndpoint = endpointProvider.GetHealthCheckEndpoint();

        // Assert
        string expectedEndpoint = $"http://localhost:{expectedPort}/healthz";
        Assert.Equal(expectedEndpoint, healthCheckEndpoint);
    }

    [Fact]
    public void GetTestingEndpoint_ReturnsCorrectEndpoint()
    {
        // Arrange
        int expectedPort = 9999;
        _portProvider.CurrentPort.Returns(expectedPort);
        var endpointProvider = new ContainerEndpointProvider(_portProvider);

        // Act
        string testingEndpoint = endpointProvider.GetTestingEndpoint();

        // Assert
        string expectedEndpoint = $"http://localhost:{expectedPort}/api/test";
        Assert.Equal(expectedEndpoint, testingEndpoint);
    }
}
