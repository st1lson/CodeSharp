namespace CodeSharp.Core.Executors;

public abstract class CodeExecutor
{
    protected static Task<string> ReadCodeFromFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        const string requiredFileExtension = ".cs";

        var fileExtension = Path.GetExtension(filePath);
        if (!fileExtension.Equals(requiredFileExtension))
        {
            throw new ArgumentException("Wrong file extension");
        }

        return File.ReadAllTextAsync(filePath, cancellationToken);
    }
}
