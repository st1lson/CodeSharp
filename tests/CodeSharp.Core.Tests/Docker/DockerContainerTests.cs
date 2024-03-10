using AutoFixture;
using CodeSharp.Core.Docker;
using CodeSharp.Core.Docker.Models;
using CodeSharp.Core.Docker.Providers;
using Docker.DotNet;
using Docker.DotNet.Models;
using NSubstitute;
using Xunit;

namespace CodeSharp.Core.Tests.Docker;

public class DockerContainerTests
{
    private readonly DockerContainer _dockerContainer;
    private readonly IContainerNameProvider _nameProvider = Substitute.For<IContainerNameProvider>();
    private readonly IContainerPortProvider _portProvider = Substitute.For<IContainerPortProvider>();
    private readonly IContainerHealthCheckProvider _healthCheckProvider = Substitute.For<IContainerHealthCheckProvider>();
    private readonly IDockerClient _dockerClient = Substitute.For<IDockerClient>();
    private readonly ContainerConfiguration _configuration = new() { Image = Image.Default };

    private readonly IFixture _fixture = new Fixture();

    public DockerContainerTests()
    {
        _dockerContainer = new DockerContainer(_configuration, _nameProvider, _portProvider, _healthCheckProvider);
    }

    [Fact]
    public async Task StartAsync_PullsImage_IfNotExists()
    {
        // Arrange
        _nameProvider.GetName().Returns("container-name");
        _portProvider.CurrentPort.Returns(1234);
        _dockerClient.Images.ListImagesAsync(Arg.Any<ImagesListParameters>(), default)
            .Returns(Task.FromResult(new List<ImagesListResponse>() as IList<ImagesListResponse>));

        var createContainerResponse = _fixture.Create<CreateContainerResponse?>();

        _dockerClient.Containers.CreateContainerAsync(_fixture.Create<CreateContainerParameters>(), CancellationToken.None)
            .Returns(createContainerResponse);

        _dockerContainer.CreateContainerAsync(CancellationToken.None)
            .Returns(Task.FromResult(createContainerResponse));

        // Act
        await _dockerContainer.StartAsync();

        // Assert
        await _dockerClient.Images.Received(1)
            .CreateImageAsync(Arg.Is<ImagesCreateParameters>(args => args.FromImage == _configuration.Image.ToString()),
                              Arg.Any<AuthConfig>(), Arg.Any<Progress<JSONMessage>>(), default);
    }

}

