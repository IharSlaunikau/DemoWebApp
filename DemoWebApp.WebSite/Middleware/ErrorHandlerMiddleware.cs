using Serilog;

namespace DemoWebApp.WebSite.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "text/html";
        return context.Response.WriteAsync(
            $"<html><body>\r\nAn error occurred while processing your request: <br><br>\r\n{exception.Message}<br><br>\r\nSee logs for more details.<br><br>\r\n</body></html>\r\n");
    }
}
