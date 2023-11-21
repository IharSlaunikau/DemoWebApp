using DemoWebApp.WebSite.Filters;
using DemoWebApp.WebSite.Settings;
using Microsoft.Extensions.Options;

namespace DemoWebApp.WebSite.Configuration;

public static class LogActionFilterConfig
{
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(provider =>
            new LogActionFilter(
                provider.GetRequiredService<ILogger<LogActionFilter>>(),
                provider.GetRequiredService<IOptions<LogSettings>>()));
    }
}
