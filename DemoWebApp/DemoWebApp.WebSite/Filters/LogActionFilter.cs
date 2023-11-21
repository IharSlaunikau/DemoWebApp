using System.Text.Json;
using DemoWebApp.WebSite.Settings;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace DemoWebApp.WebSite.Filters;

public class LogActionFilter : IAsyncActionFilter
{
    private readonly ILogger<LogActionFilter> _logger;
    private readonly LogSettings _logSettings;

    public LogActionFilter
    (
        ILogger<LogActionFilter> logger,
        IOptions<LogSettings> setting
    )
    {
        _logger = logger;
        _logSettings = setting.Value;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        OnActionExecuting(context);

        var executedContext = await next();

        await OnActionExecutedAsync(executedContext);
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var actionName = context.ActionDescriptor.DisplayName;
        var controllerName = context.Controller.GetType().Name;
        var request = context.HttpContext.Request;

        _logger.LogInformation("Starting {RequestMethod} {RequestUrl} in Controller {ControllerName}, Action {ActionName}",
            request.Method, request.GetDisplayUrl(), controllerName, actionName);

        if (_logSettings.IsLogParameters)
        {
            foreach (var parameter in context.ActionArguments)
            {
                _logger.LogInformation("Parameter [{ParameterKey}]: {ParameterValue}", parameter.Key, parameter.Value);
            }
        }
    }

    public Task OnActionExecutedAsync(ActionExecutedContext context)
    {
        var actionName = context.ActionDescriptor.DisplayName;
        var controllerName = context.Controller.GetType().Name;
        var response = context.HttpContext.Response;

        _logger.LogInformation("Finished Controller {ControllerName}, Action {ActionName} with Status Code {StatusCode}",
            controllerName, actionName, response.StatusCode);

        var responseBody = context.Result switch
        {
            ContentResult contentResult => contentResult.Content,
            ObjectResult objectResult => JsonSerializer.Serialize(objectResult.Value),
            JsonResult jsonResult => JsonSerializer.Serialize(jsonResult.Value),
            FileContentResult fileContentResult => $"File result: {fileContentResult.FileDownloadName}",
            _ => "Result type not handled in logging"
        };

        _logger.LogInformation("Response Body: {ResponseBody}", responseBody);
        return Task.CompletedTask;
    }
}
