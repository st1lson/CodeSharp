using CodeSharp.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CodeSharp.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCodeSharp<TCompilationLog, TTest, TTestLog>(this IServiceCollection services, Action<CodeSharpBuilder>? configureBuilder = null)
        where TCompilationLog : class
        where TTest : class
        where TTestLog : class
    {
        var builder = new CodeSharpBuilder(typeof(TCompilationLog), typeof(TTest), typeof(TTestLog), services);

        builder.AddDefaultImplementations();

        configureBuilder?.Invoke(builder);

        return services;
    }

    public static IServiceCollection AddCodeSharp(this IServiceCollection services, Action<CodeSharpBuilder>? configureBuilder = null)
    {
        return services.AddCodeSharp<CompilationLog, Test, TestLog>(configureBuilder);
    }
}
