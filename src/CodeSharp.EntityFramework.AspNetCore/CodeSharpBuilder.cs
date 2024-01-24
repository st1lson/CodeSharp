using CodeSharp.Core.Contracts;
using CodeSharp.Core.Executors;
using CodeSharp.Core.Services;
using CodeSharp.EntityFramework.Stores;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CodeSharp.EntityFramework.AspNetCore;

public class CodeSharpBuilder
{
    private readonly IServiceCollection _services;

    public CodeSharpBuilder(IServiceCollection services)
    {
        _services = services;
    }

    internal void AddDefaultImplementations()
    {
        AddCompilationService(typeof(ICompilationService<,>), typeof(CompilationService<,>));
        AddCompilationLogStore(typeof(ICompilationLogStore<,>), typeof(CompilationLogStore<,,>));
        AddCompileExecutor(typeof(ICompileExecutor<>), typeof(CompileExecutor<>));
    }

    public CodeSharpBuilder AddCompilationService(Type serviceType, Type implementationType)
    {
        RegisterImplementations(serviceType, implementationType);
        return this;
    }

    public CodeSharpBuilder AddCompilationLogStore(Type serviceType, Type implementationType)
    {
        RegisterImplementations(serviceType, implementationType);
        return this;
    }

    public CodeSharpBuilder AddCompileExecutor(Type serviceType, Type implementationType)
    {
        RegisterImplementations(serviceType, implementationType);
        return this;
    }

    public IServiceCollection Build()
    {
        return _services;
    }

    private void RegisterImplementations(Type serviceType, Type implementationType)
    {
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
}
