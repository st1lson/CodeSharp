﻿using Core.Docker.Providers;
using Core.Models;

namespace Core.Docker;

public class DockerConfiguration : IDockerConfiguration
{
    public Image Image { get; }
    public IDockerContainerNameProvider ContainerNameProvider { get; }
    public IDockerContainerPortProvider ContainerPortProvider { get; }

    public DockerConfiguration(
        Image image,
        IDockerContainerNameProvider containerNameProvider,
        IDockerContainerPortProvider containerPortProvider)
    {
        Image = image ?? throw new ArgumentNullException(nameof(image));
        ContainerNameProvider = containerNameProvider ?? throw new ArgumentNullException(nameof(containerNameProvider));
        ContainerPortProvider = containerPortProvider ?? throw new ArgumentNullException(nameof(containerPortProvider));
    }
}