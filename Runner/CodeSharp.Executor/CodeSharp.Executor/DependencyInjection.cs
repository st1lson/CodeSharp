using Carter;
using CodeSharp.Executor.Options;
using System.Reflection;

namespace CodeSharp.Executor;

public static class DependencyInjection
{
    public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        // Configure options
        serviceCollection.Configure<ApplicationOptions>(configuration.GetSection(ApplicationOptions.OptionsKey));

        var assembly = Assembly.GetCallingAssembly();

        serviceCollection.AddMediatR(config =>
            config.RegisterServicesFromAssembly(assembly));

        serviceCollection.AddCarter();

        return serviceCollection;
    }
}