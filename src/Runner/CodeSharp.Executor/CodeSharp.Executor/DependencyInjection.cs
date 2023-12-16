using Carter;
using CodeSharp.Executor.Options;
using System.Reflection;
using CodeSharp.Executor.Contracts.Compilation;
using CodeSharp.Executor.Infrastructure.Interfaces;
using CodeSharp.Executor.Infrastructure.Parsers;
using CodeSharp.Executor.Infrastructure.Services;

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

        serviceCollection.AddScoped<IFileService, FileService>();
        serviceCollection.AddScoped<IProcessService, ProcessService>();
        serviceCollection.AddScoped<ITestReportParser, XmlTestReportParser>();
        serviceCollection.AddScoped<ICompilationService, CompilationService>();
        serviceCollection.AddScoped<ICodeAnalysisService, CodeAnalysisService>();
        serviceCollection.AddScoped<ICodeMetricsReportParser, CodeMetricsReportParser>();
        serviceCollection.AddScoped<ICodeAnalysisReportParser, CodeAnalysisReportParser>();

        return serviceCollection;
    }
}