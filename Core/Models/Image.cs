using System.Text.RegularExpressions;

namespace Core.Models;

public record Image(string Registry, string Name, string Tag)
{
    private const string DefaultTag = "latest";

    public static Image CreateImage(string imageString)
    {
        const string pattern = @"^(?:(?<registry>[^/]+?)/)?(?<name>[^:/]+?)(?::(?<tag>.*))?$";

        const string registryKey = "registry";
        const string nameKey = "name";
        const string tagKey = "tag";

        var match = Regex.Match(imageString, pattern);
        if (!match.Success)
        {
            throw new ArgumentException();
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
}
