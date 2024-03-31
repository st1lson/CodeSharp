using CodeSharp.Core.Docker;
using CodeSharp.Core.Docker.Factories;
using CodeSharp.Core.Docker.Providers;
using CodeSharp.Core.Executors;
using CodeSharp.Core.Executors.Strategies;
using CodeSharp.Core.Models;
using CodeSharp.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CodeSharp.AspNetCore;

public class CodeSharpBuilder
{
    public IServiceCollection Services { get; }
    public Type CompilationLogType { get; }
    public Type TestType { get; }
    public Type TestLogType { get; }

    public CodeSharpBuilder(IServiceCollection services, Type compilationLogType, Type testType, Type testLogType)
    {
        Services = services;
        CompilationLogType = compilationLogType;
        TestType = testType;
        TestLogType = testLogType;
    }

    internal void AddDefaultImplementations()
    {
        // Container
        AddContainerNameProvider<RandomContainerNameProvider>();
        AddContainerEndpointProvider<ContainerEndpointProvider>();
        AddContainerPortProvider<ContainerPortProvider>();
        AddHttpContainerHealthCheckProvider<HttpContainerHealthCheckProvider>();
        AddDockerClientFactory<DockerClientFactory>();

        // Services
        AddCompilationService<CompilationService<CompilationLog>>();
        AddTestService<TestService<Test>>();
        AddTestLogService<TestLogService<TestLog>>();

        // Executors
        AddHttpCommunicationStrategy<HttpCommunicationStrategy>();
        AddCompileExecutor<CompileExecutor<CompilationLog>>();
        AddTestExecutor<TestExecutor<TestLog>>();
    }

    public CodeSharpBuilder AddContainerEndpointProvider<TEndpointProvider>() where TEndpointProvider : IContainerEndpointProvider
    {
        RegisterImplementations(typeof(IContainerEndpointProvider), typeof(TEndpointProvider));

        return this;
    }

    public CodeSharpBuilder AddContainerPortProvider<TPortProvider>() where TPortProvider : IContainerPortProvider
    {
        RegisterImplementations(typeof(IContainerPortProvider), typeof(TPortProvider));

        return this;
    }

    public CodeSharpBuilder AddContainerHealthCheckProvider<THealthCheckProvider>() where THealthCheckProvider : IContainerHealthCheckProvider
    {
        RegisterImplementations(typeof(IContainerHealthCheckProvider), typeof(THealthCheckProvider));

        return this;
    }

    public CodeSharpBuilder AddHttpContainerHealthCheckProvider<THealthCheckProvider>() where THealthCheckProvider : class, IContainerHealthCheckProvider
    {
        Services.AddHttpClient<IContainerHealthCheckProvider, THealthCheckProvider>();

        return this;
    }

    public CodeSharpBuilder AddContainerNameProvider<TNameProvider>() where TNameProvider : IContainerNameProvider
    {
        RegisterImplementations(typeof(IContainerNameProvider), typeof(TNameProvider));

        return this;
    }

    public CodeSharpBuilder AddDockerClientFactory<TDockerClientFactory>() where TDockerClientFactory : IDockerClientFactory
    {
        RegisterImplementations(typeof(IDockerClientFactory), typeof(DockerClientFactory));

        return this;
    }

    public CodeSharpBuilder AddDockerContainer<TDockerContainer>() where TDockerContainer : IDockerContainer
    {
        RegisterImplementations(typeof(IDockerContainer), typeof(TDockerContainer));

        return this;
    }

    public CodeSharpBuilder AddCompilationService<TCompilationService>() where TCompilationService : class
    {
        var keyType = GetKeyType(CompilationLogType);

        var serviceType = typeof(ICompilationService<,>).MakeGenericType(CompilationLogType, keyType);
        var implementationType = typeof(TCompilationService);

        RegisterImplementations(serviceType, implementationType);

        return this;
    }

    public CodeSharpBuilder AddTestService<TTestService>() where TTestService : class
    {
        var keyType = GetKeyType(TestType);

        var serviceType = typeof(ITestService<,,>).MakeGenericType(TestType, TestLogType, keyType);
        var implementationType = typeof(TTestService);

        RegisterImplementations(serviceType, implementationType);

        return this;
    }

    public CodeSharpBuilder AddTestLogService<TTestLogService>() where TTestLogService : class
    {
        var keyType = GetKeyType(TestLogType);

        var serviceType = typeof(ITestLogService<,>).MakeGenericType(TestLogType, keyType);
        var implementationType = typeof(TTestLogService);

        RegisterImplementations(serviceType, implementationType);

        return this;
    }

    public CodeSharpBuilder AddHttpCommunicationStrategy<THttpCommunicationStrategy>() where THttpCommunicationStrategy : class, ICommunicationStrategy
    {
        Services.AddHttpClient<ICommunicationStrategy, THttpCommunicationStrategy>();

        return this;
    }

    public CodeSharpBuilder AddCompileExecutor<TCompileExecutor>() where TCompileExecutor : class
    {
        var codeExecutorType = typeof(ICompileExecutor<>).MakeGenericType(CompilationLogType);
        var implementationType = typeof(TCompileExecutor);

        RegisterImplementations(codeExecutorType, implementationType);

        return this;
    }

    public CodeSharpBuilder AddTestExecutor<TTestExecutor>() where TTestExecutor : class
    {
        var testExecutorType = typeof(ITestExecutor<>).MakeGenericType(TestLogType);
        var implementationType = typeof(TTestExecutor);

        RegisterImplementations(testExecutorType, implementationType);

        return this;
    }

    protected void RegisterImplementations(Type serviceType, Type implementationType)
    {
        if (!serviceType.IsAssignableFrom(implementationType))
        {
            throw new ArgumentException($"{serviceType} must implement {implementationType}.");
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
                Services.AddScoped(serviceType, implType);
            }
        }
        else
        {
            Services.AddScoped(serviceType, implementationType);
        }
    }

    protected static Type GetKeyType(Type entity)
    {
        var idProperty = entity.GetProperties().FirstOrDefault(p => p.Name == "Id")
            ?? throw new InvalidOperationException($"The type {entity} must have a property named 'Id'.");

        return idProperty.PropertyType;
    }
}
