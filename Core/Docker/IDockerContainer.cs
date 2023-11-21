namespace Core.Docker;

public interface IDockerContainer : IDisposable
{
    Task StartAsync(CancellationToken cancellationToken = default);
    Task WaitToBeReadyAsync(CancellationToken cancellationToken = default);
}
