﻿namespace Core.Docker.Providers;

public interface IDockerContainerPortProvider
{
    int CurrentPort { get; }
    void AcquirePort();
    void ReleasePort();
}