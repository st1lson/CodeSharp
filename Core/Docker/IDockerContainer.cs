namespace Core.Docker;

public interface IDockerContainer : IDisposable
{
    Task StartAsync(CancellationToken cancellationToken = default);
    Task WaitToBeReadyAsync(CancellationToken cancellationToken = default);
    Task StopAsync(CancellationToken cancellationToken = default);
}
