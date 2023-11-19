namespace Core.Docker.Providers;

public interface IDockerContainerPortProvider
{
    string CurrentPort { get; }
    void ReleasePort();
}