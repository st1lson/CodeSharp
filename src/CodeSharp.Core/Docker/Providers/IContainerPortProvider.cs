namespace CodeSharp.Core.Docker.Providers;

public interface IContainerPortProvider
{
    int CurrentPort { get; }
    void AcquirePort();
    void ReleasePort();
}