using System.Reflection;
using Carter;

namespace CodeSharp.Executor;

public static class DependencyInjection
{
    public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
    {
        var assembly = Assembly.GetCallingAssembly();
        
        serviceCollection.AddMediatR(config =>
            config.RegisterServicesFromAssembly(assembly));
        
        serviceCollection.AddCarter();

        return serviceCollection;
    }
}