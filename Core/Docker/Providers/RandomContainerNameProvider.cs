namespace Core.Docker.Providers;

public class RandomContainerNameProvider : IContainerNameProvider
{
    private string? _containerName;

    public string GetName()
    {
        _containerName ??= $"{Random.Shared.Next(10000)}";

        return _containerName;
    }
}
