using System.Text.RegularExpressions;

namespace CodeSharp.Core.Docker.Models;

public partial record Image(string Registry, string Name, string Tag)
{
    private const string DefaultTag = "latest";

    public static Image Default => CreateImage("codesharp.executor:latest");

    public static Image CreateImage(string imageString)
    {
        const string registryKey = "registry";
        const string nameKey = "name";
        const string tagKey = "tag";

        var match = ImageRegex().Match(imageString);
        if (!match.Success)
        {
            throw new ArgumentException($"Invalid image string format. Expected format 'registry/name:tag'. Received: '{imageString}'", nameof(imageString));
        }

        string registry = match.Groups[registryKey].Value;
        string name = match.Groups[nameKey].Value;
        string tag = match.Groups[tagKey].Value;

        if (string.IsNullOrEmpty(tag))
        {
            tag = DefaultTag;
        }

        return new Image(registry, name, tag);
    }

    public override string ToString()
    {
        if (string.IsNullOrEmpty(Registry))
        {
            return $"{Name}:{Tag}";
        }

        return $"{Registry}/{Name}:{Tag}";
    }

    [GeneratedRegex("^(?:(?<registry>[^/]+?)/)?(?<name>[^:/]+?)(?::(?<tag>.*))?$", RegexOptions.Compiled, 5000)]
    private static partial Regex ImageRegex();
}
