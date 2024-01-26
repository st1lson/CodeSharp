using CodeSharp.Core.Docker;
using CodeSharp.Core.Docker.Providers;
using CodeSharp.Core.Executors;
using CodeSharp.Core.Models;
using CodeSharp.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CodeSharp.AspNetCore;

public class CodeSharpBuilder<TCompilationLog, TTest, TTestLog>
{
    private readonly IServiceCollection _services;

    public CodeSharpBuilder(IServiceCollection services)
    {
        _services = services;
    }

    internal void AddDefaultImplementations()
    {
        AddContainerEndpointProvider<ContainerEndpointProvider>();
        AddContainerPortProvider<ContainerPortProvider>();
        AddContainerHealthCheckProvider<HttpContainerHealthCheckProvider>();
        AddDockerContainer<DockerContainer>();
        AddCompilationService<CompilationService<CompilationLog>>();
        AddCompileExecutor<CompileExecutor<CompilationLog>>();
    }

    public CodeSharpBuilder<TCompilationLog, TTest, TTestLog> AddContainerEndpointProvider<TEndpointProvider>() where TEndpointProvider : IContainerEndpointProvider
    {
        RegisterImplementations(typeof(TEndpointProvider), typeof(IContainerEndpointProvider));

        return this;
    }

    public CodeSharpBuilder<TCompilationLog, TTest, TTestLog> AddContainerPortProvider<TPortProvider>() where TPortProvider : IContainerPortProvider
    {
        RegisterImplementations(typeof(TPortProvider), typeof(IContainerPortProvider));

        return this;
    }

    public CodeSharpBuilder<TCompilationLog, TTest, TTestLog> AddContainerHealthCheckProvider<THealthCheckProvider>() where THealthCheckProvider : IContainerHealthCheckProvider
    {
        RegisterImplementations(typeof(THealthCheckProvider), typeof(IContainerHealthCheckProvider));

        return this;
    }

    public CodeSharpBuilder<TCompilationLog, TTest, TTestLog> AddContainerNameProvider<TNameProvider>() where TNameProvider : IContainerNameProvider
    {
        RegisterImplementations(typeof(TNameProvider), typeof(IContainerNameProvider));

        return this;
    }


    public CodeSharpBuilder<TCompilationLog, TTest, TTestLog> AddDockerContainer<TDockerContainer>() where TDockerContainer : IDockerContainer
    {
        RegisterImplementations(typeof(TDockerContainer), typeof(IDockerContainer));

        return this;
    }

    public CodeSharpBuilder<TCompilationLog, TTest, TTestLog> AddCompilationService<TCompilationService>() where TCompilationService : class
    {
        var compilationLogType = typeof(TCompilationLog);

        var keyType = GetKeyType(compilationLogType);

        var serviceType = typeof(ICompilationService<,>).MakeGenericType(compilationLogType, keyType);
        var implementationType = typeof(TCompilationService);

        RegisterImplementations(serviceType, implementationType);

        return this;
    }

    //public CodeSharpBuilder<TCompilationLog, TTest, TTestLog> AddCompilationLogStore<TStore>() where TStore : class
    //{
    //    var compilationLogType = typeof(TCompilationLog);

    //    var keyType = GetKeyType(compilationLogType);

    //    var storeType = typeof(ICompilationLogStore<,>).MakeGenericType(compilationLogType, keyType);
    //    var implementationType = typeof(TStore);

    //    RegisterImplementations(storeType, implementationType);

    //    return this;
    //}

    public CodeSharpBuilder<TCompilationLog, TTest, TTestLog> AddCompileExecutor<TCompileExecutor>() where TCompileExecutor : class
    {
        var codeExecutorType = typeof(ICompileExecutor<>).MakeGenericType(typeof(TCompilationLog));
        var implementationType = typeof(TCompileExecutor);

        RegisterImplementations(codeExecutorType, implementationType);

        return this;
    }

    public CodeSharpBuilder<TCompilationLog, TTest, TTestLog> AddTestExecutor<TTestExecutor>() where TTestExecutor : class
    {
        var testExecutorType = typeof(ITestExecutor<>).MakeGenericType(typeof(TCompilationLog));
        var implementationType = typeof(TTestExecutor);

        RegisterImplementations(testExecutorType, implementationType);

        return this;
    }

    protected void RegisterImplementations(Type serviceType, Type implementationType)
    {
        if (!serviceType.IsAssignableFrom(implementationType))
        {
            throw new ArgumentException($"{implementationType} must implement {serviceType}.");
        }

        var assembly = Assembly.GetExecutingAssembly();

        var implementations = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface &&
                        serviceType.IsAssignableFrom(t) && t != implementationType)
            .ToList();

        if (implementations.Any())
        {
            foreach (var implType in implementations)
            {
                _services.AddScoped(serviceType, implType);
            }
        }
        else
        {
            _services.AddScoped(serviceType, implementationType);
        }
    }

    protected static Type GetKeyType(Type entity)
    {
        var idProperty = entity.GetProperties().FirstOrDefault(p => p.Name == "Id")
            ?? throw new InvalidOperationException($"The type {entity} must have a property named 'Id'.");

        return idProperty.PropertyType;
    }
}
