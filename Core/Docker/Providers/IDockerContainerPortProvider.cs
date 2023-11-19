namespace Core.Docker.Providers;

public interface IDockerContainerPortProvider
{
    int CurrentPort { get; }
    void ReleasePort();
}