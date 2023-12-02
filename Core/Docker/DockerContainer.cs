using Core.Docker.Exceptions;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace Core.Docker;

public class DockerContainer : IDockerContainer
{
    private readonly IDockerConfiguration _configuration;

    private IDockerClient? _dockerClient;
    private string? _containerId;

    public DockerContainer(
        IDockerConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        _dockerClient = new DockerClientConfiguration()
            .CreateClient();

        var dockerImageExists = await ImageExistsAsync(cancellationToken);
        if (!dockerImageExists)
        {
            await PullRunnerImageAsync(cancellationToken);
        }

        var container = await CreateContainerAsync(cancellationToken);
        if (container is null)
        {
            return;
        }

        _containerId = container.ID;
        await _dockerClient.Containers.StartContainerAsync(container.ID, new ContainerStartParameters(), cancellationToken);
    }

    public async Task WaitToBeReadyAsync(CancellationToken cancellationToken = default)
    {
        const int retries = 5;

        var delayTask = Task.Delay(1000, cancellationToken);
        using var httpClient = new HttpClient();
        for (int i = 0; i < retries; i++)
        {
            try
            {
                var healthCheckEndpoint = _configuration.ContainerEndpointProvider.GetHealthCheckEndpoint();

                var healthCheckResponse = await httpClient.GetAsync(healthCheckEndpoint, cancellationToken);

                if (healthCheckResponse.IsSuccessStatusCode)
                {
                    return;
                }

                await delayTask;
            }
            catch (Exception)
            {
                await delayTask;
            }
        }

        throw new HealthcheckFailedException("Failed to connect to the server");
    }

    private async Task<bool> ImageExistsAsync(CancellationToken cancellationToken = default)
    {
        var imagesList = await _dockerClient!.Images.ListImagesAsync(new ImagesListParameters(), cancellationToken);
        return imagesList.Any(image => image.RepoTags.Contains(_configuration.Image.ToString()));
    }

    private Task PullRunnerImageAsync(CancellationToken cancellationToken = default)
    {
        return _dockerClient!.Images.CreateImageAsync(new ImagesCreateParameters
        {
            FromImage = _configuration.Image.ToString()
        }, new AuthConfig(), new Progress<JSONMessage>(), cancellationToken);
    }

    private Task<CreateContainerResponse?> CreateContainerAsync(CancellationToken cancellationToken = default)
    {
        _configuration.ContainerPortProvider.AcquirePort();

        var createContainerParameters = new CreateContainerParameters
        {
            Image = _configuration.Image.ToString(),
            Name = _configuration.ContainerNameProvider.GetName(),
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
                                HostPort = _configuration.ContainerPortProvider.CurrentPort.ToString()
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
            // TODO: Add custom exception
            throw new Exception("The container is not running");
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
            var containerName = _configuration.ContainerNameProvider.GetName();
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
        _configuration.ContainerPortProvider.ReleasePort();
    }
}
