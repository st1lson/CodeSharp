using CodeSharp.Core.Docker.Models;
using CodeSharp.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CodeSharp.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCodeSharp<TCompilationLog, TTest, TTestLog>(this IServiceCollection services, Action<ContainerConfiguration>? configuration = null)
        where TCompilationLog : class
        where TTest : class
        where TTestLog : class
    {
        var builder = new CodeSharpBuilder<TCompilationLog, TTest, TTestLog>(services);

        services.AddSingleton(provider =>
        {
            var defaultConfig = new ContainerConfiguration();
            configuration?.Invoke(defaultConfig);

            return defaultConfig;
        });

        builder.AddDefaultImplementations();

        return services;
    }

    public static IServiceCollection AddCodeSharp(this IServiceCollection services, Action<ContainerConfiguration>? configuration = null)
    {
        return services.AddCodeSharp<CompilationLog, Test, TestLog>(configuration);
    }
}
