namespace Core.Docker.Providers;

public class RandomDockerContainerNameProvider : IDockerContainerNameProvider
{
    private string? _containerName;

    public string GetName()
    {
        _containerName ??= $"{Random.Shared.Next(10000)}";

        return _containerName;
    }
}
