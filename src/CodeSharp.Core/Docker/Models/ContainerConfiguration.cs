namespace CodeSharp.Core.Docker.Models;

public class ContainerConfiguration
{
    public Image Image { get; set; } = Image.Default;
    public string ExposedPort { get; set; } = "80";
}