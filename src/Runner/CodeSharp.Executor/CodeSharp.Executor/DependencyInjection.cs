using Carter;
using CodeSharp.Executor.Common.Behaviors;
using CodeSharp.Executor.Infrastructure.Interfaces;
using CodeSharp.Executor.Infrastructure.Parsers;
using CodeSharp.Executor.Infrastructure.Services;
using CodeSharp.Executor.Options;
using FluentValidation;
using MediatR;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace CodeSharp.Executor;

public static class DependencyInjection
{
    public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        // Configure options
        serviceCollection.Configure<ApplicationOptions>(configuration.GetSection(ApplicationOptions.OptionsKey));

        serviceCollection.AddHealthChecks();

        var assembly = Assembly.GetCallingAssembly();

        serviceCollection.AddValidatorsFromAssembly(assembly);

        serviceCollection.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        serviceCollection.AddCarter();

        serviceCollection.AddScoped<IFileService, FileService>();
        serviceCollection.AddScoped<IProcessService, ProcessService>();
        serviceCollection.AddScoped<ITestReportParser, XmlTestReportParser>();
        serviceCollection.AddScoped<ICompilationService, CompilationService>();
        serviceCollection.AddScoped<ICodeAnalysisService, CodeAnalysisService>();
        serviceCollection.AddScoped<ICodeMetricsReportParser, CodeMetricsReportParser>();
        serviceCollection.AddScoped<ICodeAnalysisReportParser, CodeAnalysisReportParser>();

        serviceCollection.AddSwaggerGen(options =>
        {
            options.MapType<TimeSpan>(() => new OpenApiSchema
            {
                Type = "string",
                Example = new OpenApiString("00:00:00")
            });
        });

        return serviceCollection;
    }
}