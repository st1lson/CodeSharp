namespace Core.Docker;

public interface IDockerContainer : IDisposable
{
    Task StartAsync(CancellationToken cancellationToken = default);
    Task EnsureCreatedAsync(CancellationToken cancellationToken = default);
    Task StopAsync(CancellationToken cancellationToken = default);
}
