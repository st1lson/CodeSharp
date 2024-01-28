using CodeSharp.Core.Docker.Models;
using CodeSharp.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CodeSharp.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static CodeSharpBuilder<TCompilationLog, TTest, TTestLog> AddCodeSharp<TCompilationLog, TTest, TTestLog>(
        this IServiceCollection services,
        Action<ContainerConfiguration>? configuration = null)
        where TCompilationLog : class
        where TTest : class
        where TTestLog : class
    {
        var builder = new CodeSharpBuilder<TCompilationLog, TTest, TTestLog>(services);

        services.AddSingleton(_ =>
        {
            var defaultConfig = new ContainerConfiguration();
            configuration?.Invoke(defaultConfig);

            return defaultConfig;
        });

        builder.AddDefaultImplementations();

        return builder;
    }

    public static CodeSharpBuilder<CompilationLog, Test, TestLog> AddCodeSharp(this IServiceCollection services, Action<ContainerConfiguration>? configuration = null)
    {
        return services.AddCodeSharp<CompilationLog, Test, TestLog>(configuration);
    }
}
