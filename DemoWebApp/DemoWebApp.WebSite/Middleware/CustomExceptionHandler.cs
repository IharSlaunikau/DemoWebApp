using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using DemoWebApp.WebSite.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace DemoWebApp.WebSite.Middleware;

[ExcludeFromCodeCoverage]
public class CustomExceptionHandler : IMiddleware
{
    private readonly ILogger<CustomExceptionHandler> _logger;

    public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
        var thrownException = errorFeature?.Error;

        if (thrownException != null)
        {
            _logger.LogError(thrownException, "An unhandled exception has occurred.");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var errorResponse = new ErrorResponse
            {
                StatusCode = context.Response.StatusCode,
                Message = $"An error has occurred: {thrownException.Message}. Please check the logs for more information."
            };

            var jsonErrorResponse = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(jsonErrorResponse);
        }
        else
        {
            await next(context);
        }
    }
}
