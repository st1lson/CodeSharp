using Core.Docker.Models;
using Core.Docker.Providers;
using Core.Services;

namespace CodeSharp.Samples.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddCodeSharp(this IServiceCollection services)
    {
        services.AddScoped<IContainerPortProvider, ContainerPortProvider>();
        services.AddScoped<IContainerEndpointProvider, ContainerEndpointProvider>();
        services.AddScoped<IContainerHealthCheckProvider, HttpContainerHealthCheckProvider>();
        services.AddScoped<IContainerNameProvider, RandomContainerNameProvider>();

        ContainerConfiguration configuration = new ContainerConfiguration
        {
            Image = Image.CreateImage("codesharp.executor:latest")
        };

        services.AddSingleton(configuration);

        services.AddScoped<ICompilationService, CompilationService>();
        services.AddScoped<ITestingService, TestingService>();

        return services;
    }
}
