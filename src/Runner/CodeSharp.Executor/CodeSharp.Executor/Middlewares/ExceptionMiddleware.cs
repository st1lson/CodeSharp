using CodeSharp.Executor.Contracts.Shared;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace CodeSharp.Executor.Middlewares;

internal sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An exception occurred");
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;

        var (code, errors) = exception switch
        {
            _ => (HttpStatusCode.InternalServerError, new[] { ApplicationError.Unexpected() })
        };

        context.Response.StatusCode = (int)code;

        string response = JsonSerializer.Serialize(
            new ApiErrorResponse(errors),
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

        await context.Response.WriteAsync(response);
    }
}