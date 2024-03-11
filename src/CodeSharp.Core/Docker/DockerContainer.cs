using CodeSharp.Core.Docker.Exceptions;
using CodeSharp.Core.Docker.Factories;
using CodeSharp.Core.Docker.Models;
using CodeSharp.Core.Docker.Providers;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace CodeSharp.Core.Docker;

public class DockerContainer : IDockerContainer
{
    private readonly ContainerConfiguration _configuration;
    private readonly IContainerNameProvider _containerNameProvider;
    private readonly IContainerPortProvider _containerPortProvider;
    private readonly IContainerHealthCheckProvider _containerHealthCheckProvider;
    private readonly IDockerClientFactory _dockerClientFactory;

    private IDockerClient? _dockerClient;
    private string? _containerId;

    public DockerContainer(
        ContainerConfiguration configuration,
        IContainerNameProvider containerNameProvider,
        IContainerPortProvider containerPortProvider,
        IContainerHealthCheckProvider containerHealthCheckProvider,
        IDockerClientFactory dockerClientFactory)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _containerNameProvider = containerNameProvider ?? throw new ArgumentNullException(nameof(containerNameProvider));
        _containerPortProvider = containerPortProvider ?? throw new ArgumentNullException(nameof(containerPortProvider));
        _containerHealthCheckProvider = containerHealthCheckProvider ?? throw new ArgumentNullException(nameof(containerHealthCheckProvider));
        _dockerClientFactory = dockerClientFactory ?? throw new ArgumentNullException(nameof(dockerClientFactory));
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        _dockerClient ??= _dockerClientFactory.CreateClient();

        var dockerImageExists = await ImageExistsAsync(cancellationToken);
        if (!dockerImageExists)
        {
            await PullRunnerImageAsync(cancellationToken);
        }

        var container = await CreateAsync(cancellationToken)
            ?? throw new DockerContainerException("Failed to create docker container");

        _containerId = container.ID;
        await _dockerClient.Containers.StartContainerAsync(container.ID, new ContainerStartParameters(), cancellationToken);

        await _containerHealthCheckProvider.EnsureCreatedAsync(cancellationToken);
    }

    public Task EnsureCreatedAsync(CancellationToken cancellationToken = default)
    {
        if (_dockerClient is null)
        {
            throw new DockerContainerException("The container is not running");
        }

        return _containerHealthCheckProvider.EnsureCreatedAsync(cancellationToken);
    }

    private async Task<bool> ImageExistsAsync(CancellationToken cancellationToken = default)
    {
        var imagesList = await _dockerClient!.Images.ListImagesAsync(new ImagesListParameters(), cancellationToken);

        var imageName = _configuration.Image.ToString();
        return imagesList.Any(image => image.RepoTags.Contains(imageName));
    }

    private Task PullRunnerImageAsync(CancellationToken cancellationToken = default)
    {
        return _dockerClient!.Images.CreateImageAsync(new ImagesCreateParameters
        {
            FromImage = _configuration.Image.ToString()
        }, new AuthConfig(), new Progress<JSONMessage>(), cancellationToken);
    }

    public Task<CreateContainerResponse?> CreateAsync(CancellationToken cancellationToken = default)
    {
        _containerPortProvider.AcquirePort();

        var createContainerParameters = new CreateContainerParameters
        {
            Image = _configuration.Image.ToString(),
            Name = _containerNameProvider.GetName(),
            Tty = true,
            AttachStdout = true,
            AttachStderr = true,
            ExposedPorts = new Dictionary<string, EmptyStruct> { { "80", default(EmptyStruct) } },
            HostConfig = new HostConfig
            {
                PortBindings = new Dictionary<string, IList<PortBinding>>
                {
                    { "80", new List<PortBinding>
                        { new()
                            {
                                HostPort = _containerPortProvider.CurrentPort.ToString()
                            }
                        }
                    }
                }
            }
        };

        return _dockerClient!.Containers.CreateContainerAsync(createContainerParameters, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        if (_dockerClient is null)
        {
            throw new DockerContainerException("The container is not running");
        }

        return _dockerClient.Containers.StopContainerAsync(_containerId, new ContainerStopParameters(), cancellationToken);
    }

    private bool _disposed;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            var containerName = _containerNameProvider.GetName();
            if (!string.IsNullOrEmpty(containerName))
            {
                CleanupAsync().Wait();
            }

            _dockerClient?.Dispose();
        }

        _disposed = true;
    }

    private async Task CleanupAsync()
    {
        if (_dockerClient is null)
        {
            return;
        }

        await StopAsync();
        await _dockerClient.Containers.RemoveContainerAsync(_containerId, new ContainerRemoveParameters { Force = true });
        _containerPortProvider.ReleasePort();
    }
}
