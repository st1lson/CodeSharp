namespace Core.Docker.Models;

public class ContainerConfiguration
{
    public required Image Image { get; set; }
    public string ExposedPort = "80";
}