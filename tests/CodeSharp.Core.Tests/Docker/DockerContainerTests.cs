using AutoFixture;
using CodeSharp.Core.Docker;
using CodeSharp.Core.Docker.Exceptions;
using CodeSharp.Core.Docker.Factories;
using CodeSharp.Core.Docker.Models;
using CodeSharp.Core.Docker.Providers;
using Docker.DotNet;
using Docker.DotNet.Models;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace CodeSharp.Core.Tests.Docker;

public class DockerContainerTests
{
    private readonly DockerContainer _dockerContainer;
    private readonly IContainerNameProvider _nameProvider = Substitute.For<IContainerNameProvider>();
    private readonly IContainerPortProvider _portProvider = Substitute.For<IContainerPortProvider>();
    private readonly IContainerHealthCheckProvider _healthCheckProvider = Substitute.For<IContainerHealthCheckProvider>();
    private readonly IDockerClient _dockerClient = Substitute.For<IDockerClient>();
    private readonly IDockerClientFactory _dockerClientFactory = Substitute.For<IDockerClientFactory>();
    private readonly ContainerConfiguration _configuration = new() { Image = Image.Default };

    private readonly IFixture _fixture = new Fixture();

    public DockerContainerTests()
    {
        _dockerClientFactory.CreateClient().Returns(_dockerClient);
        _nameProvider.GetName().Returns(_fixture.Create<string>());

        _dockerContainer = new DockerContainer(
            _configuration,
            _nameProvider,
            _portProvider,
            _healthCheckProvider,
            _dockerClientFactory);
    }

    [Fact]
    public async Task StartAsync_WhenImageExists_DoesNotPullImage()
    {
        // Arrange
        var cancellationToken = new CancellationToken();

        _dockerClient.Images.ListImagesAsync(Arg.Any<ImagesListParameters>(), cancellationToken)
            .Returns(Task.FromResult<IList<ImagesListResponse>>(new List<ImagesListResponse>
            {
                new() { RepoTags = new List<string> { _configuration.Image.ToString() } }
            }));

        var createContainerResponse = _fixture.Create<CreateContainerResponse>();

        _dockerClient.Containers.CreateContainerAsync(Arg.Any<CreateContainerParameters>(), cancellationToken)
            .Returns(Task.FromResult(createContainerResponse));

        // Act
        await _dockerContainer.StartAsync(cancellationToken);

        // Assert
        await _dockerClient.Images.DidNotReceive().CreateImageAsync(
            Arg.Any<ImagesCreateParameters>(),
            Arg.Any<AuthConfig>(),
            Arg.Any<Progress<JSONMessage>>(),
            cancellationToken);
        await _dockerClient.Containers.Received(1).StartContainerAsync(
            Arg.Is<string>(id => id == createContainerResponse.ID),
            Arg.Any<ContainerStartParameters>(),
            cancellationToken);
    }

    [Fact]
    public async Task StartAsync_WhenImageDoesNotExist_PullsImage()
    {
        // Arrange
        var cancellationToken = new CancellationToken();

        _dockerClient.Images.ListImagesAsync(Arg.Any<ImagesListParameters>(), cancellationToken)
            .Returns(Task.FromResult<IList<ImagesListResponse>>(new List<ImagesListResponse>()));

        var createContainerResponse = _fixture.Create<CreateContainerResponse>();

        _dockerClient.Containers.CreateContainerAsync(Arg.Any<CreateContainerParameters>(), cancellationToken)
            .Returns(Task.FromResult(createContainerResponse));

        // Act
        await _dockerContainer.StartAsync(cancellationToken);

        // Assert
        await _dockerClient.Images.Received(1).CreateImageAsync(
            Arg.Any<ImagesCreateParameters>(),
            Arg.Any<AuthConfig>(),
            Arg.Any<Progress<JSONMessage>>(),
            cancellationToken);
        await _dockerClient.Containers.Received(1).StartContainerAsync(
            Arg.Is<string>(id => id == createContainerResponse.ID),
            Arg.Any<ContainerStartParameters>(),
            cancellationToken);
    }

    [Fact]
    public async Task StartAsync_WhenImagePullFails_ThrowsException()
    {
        // Arrange
        var cancellationToken = new CancellationToken();

        _dockerClient.Images.ListImagesAsync(Arg.Any<ImagesListParameters>(), cancellationToken)
            .Returns(Task.FromResult<IList<ImagesListResponse>>(new List<ImagesListResponse>()));
        _dockerClient.Images.CreateImageAsync(Arg.Any<ImagesCreateParameters>(), Arg.Any<AuthConfig>(), Arg.Any<Progress<JSONMessage>>(), cancellationToken)
            .Throws(new DockerContainerException("Failed to pull image"));

        // Act & Assert
        await Assert.ThrowsAsync<DockerContainerException>(() => _dockerContainer.StartAsync(cancellationToken));
    }


    [Fact]
    public async Task EnsureCreatedAsync_WhenCalled_InvokesHealthCheckProvider()
    {
        // Arrange
        var cancellationToken = new CancellationToken();

        var createContainerResponse = _fixture.Create<CreateContainerResponse>();

        _dockerClient.Containers.CreateContainerAsync(Arg.Any<CreateContainerParameters>(), cancellationToken)
            .Returns(Task.FromResult(createContainerResponse));

        await _dockerContainer.StartAsync(cancellationToken);

        // Act
        await _dockerContainer.EnsureCreatedAsync(cancellationToken);

        // Assert
        await _healthCheckProvider.Received(2).EnsureCreatedAsync(cancellationToken);
    }

    [Fact]
    public async Task EnsureCreatedAsync_ThrowsExceptionIfDockerClientIsNull()
    {
        // Arrange
        var cancellationToken = new CancellationToken();

        // Act & Assert
        await Assert.ThrowsAsync<DockerContainerException>(() => new DockerContainer(
            _configuration,
            _nameProvider,
            _portProvider,
            _healthCheckProvider,
            _dockerClientFactory).EnsureCreatedAsync(cancellationToken));
    }

    [Fact]
    public async Task StopAsync_WhenCalled_StopsAndRemovesContainer()
    {
        // Arrange
        var cancellationToken = new CancellationToken();

        var createContainerResponse = _fixture.Create<CreateContainerResponse>();

        _dockerClient.Containers.CreateContainerAsync(Arg.Any<CreateContainerParameters>(), cancellationToken)
            .Returns(Task.FromResult(createContainerResponse));

        await _dockerContainer.StartAsync(cancellationToken);

        // Act
        await _dockerContainer.StopAsync(cancellationToken);

        // Assert
        await _dockerClient.Containers.Received(1).StopContainerAsync(createContainerResponse.ID, Arg.Any<ContainerStopParameters>(), cancellationToken);
    }

    [Fact]
    public async Task StopAsync_ThrowsExceptionIfDockerClientIsNull()
    {
        // Arrange
        var cancellationToken = new CancellationToken();

        // Act & Assert
        await Assert.ThrowsAsync<DockerContainerException>(() => new DockerContainer(
            _configuration,
            _nameProvider,
            _portProvider,
            _healthCheckProvider,
            _dockerClientFactory).StopAsync(cancellationToken));
    }

    [Fact]
    public async Task Dispose_CleansUpAndDisposesResources()
    {
        // Arrange
        var cancellationToken = new CancellationToken();

        var createContainerResponse = _fixture.Create<CreateContainerResponse>();
        _dockerClient.Containers.CreateContainerAsync(Arg.Any<CreateContainerParameters>(), cancellationToken)
            .Returns(Task.FromResult(createContainerResponse));
        _dockerClient.Images.ListImagesAsync(Arg.Any<ImagesListParameters>(), cancellationToken)
            .Returns(Task.FromResult<IList<ImagesListResponse>>(new List<ImagesListResponse>()));
        await _dockerContainer.StartAsync(cancellationToken);

        // Act
        _dockerContainer.Dispose();

        // Assert
        await _dockerClient.Containers.Received(1).StopContainerAsync(createContainerResponse.ID, Arg.Any<ContainerStopParameters>(), cancellationToken);
        await _dockerClient.Containers.Received(1).RemoveContainerAsync(createContainerResponse.ID, Arg.Any<ContainerRemoveParameters>(), cancellationToken);
        _portProvider.Received(1).ReleasePort();
        _dockerClient.Received(1).Dispose();
    }
}