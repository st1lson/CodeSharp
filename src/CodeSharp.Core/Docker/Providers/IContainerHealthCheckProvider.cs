﻿namespace CodeSharp.Core.Docker.Providers;

public interface IContainerHealthCheckProvider
{
    Task EnsureCreatedAsync(CancellationToken cancellationToken = default);
}
