namespace CodeSharp.Samples.WebAPI;

public static class DependencyInjection
{
    public static IServiceProvider InitDbData(this IServiceProvider serviceProvider)
    {
        // Mock db data

        return serviceProvider;
    }
}
