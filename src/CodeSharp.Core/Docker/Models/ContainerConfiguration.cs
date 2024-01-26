namespace CodeSharp.Core.Docker.Models;

public record ContainerConfiguration
{
    public Image Image { get; init; } = Image.Default;
    public string ExposedPort { get; init; } = "80";
}