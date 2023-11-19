﻿using Core.Models;
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
                    { "80", new List<PortBinding> { new PortBinding { HostPort = "8080" } } }
                }
            }
        };

        return _dockerClient!.Containers.CreateContainerAsync(createContainerParameters, cancellationToken);
    }

    // TODO: Implement disposable pattern
    public void Dispose()
    {
        var containerName = _configuration.ContainerNameProvider.GetName();
        if (!string.IsNullOrEmpty(containerName))
        {
            CleanupAsync().Wait();
        }

        _dockerClient?.Dispose();
    }

    private async Task CleanupAsync()
    {
        if (_dockerClient is null)
        {
            return;
        }

        await _dockerClient.Containers.StopContainerAsync(_containerId, new ContainerStopParameters());
        await _dockerClient.Containers.RemoveContainerAsync(_containerId, new ContainerRemoveParameters { Force = true });
    }
}