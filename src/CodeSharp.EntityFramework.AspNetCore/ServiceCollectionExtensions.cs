using Microsoft.Extensions.DependencyInjection;

namespace CodeSharp.EntityFramework.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCodeSharp(this IServiceCollection services, Action<CodeSharpBuilder>? configureBuilder = null)
    {
        var builder = new CodeSharpBuilder(services);

        builder.AddDefaultImplementations();

        configureBuilder?.Invoke(builder);

        return services;
    }
}
