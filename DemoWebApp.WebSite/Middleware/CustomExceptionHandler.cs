using Microsoft.AspNetCore.Diagnostics;

namespace DemoWebApp.WebSite.Middleware;

public class CustomExceptionHandler
{
    private readonly ILogger<CustomExceptionHandler> _logger;

    public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(HttpContext context)
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception != null)
        {
            _logger.LogError(exception, "An unhandled exception has occurred.");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsync("An error has occurred. Please check the logs for more information.");
        }
    }
}
