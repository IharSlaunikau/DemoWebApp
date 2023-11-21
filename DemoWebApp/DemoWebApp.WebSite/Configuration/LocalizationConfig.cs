using System.Globalization;
using DemoWebApp.WebSite.Filters;
using DemoWebApp.WebSite.Models;
using DemoWebApp.WebSite.Settings;
using Microsoft.AspNetCore.Localization;

namespace DemoWebApp.WebSite.Configuration;

public static class LocalizationConfig
{
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        var localizationSettings = configuration.GetSection("LocalizationSettings").Get<LocalizationSettings>();
        services.Configure<LocalizationSettings>(configuration.GetSection("LocalizationSettings"));

        services
            .AddLocalization(options => { options.ResourcesPath = "Resources"; })
            .AddControllersWithViews(options =>
            {
                options.Filters.AddService<LogActionFilter>();
            })
            .AddViewLocalization()
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResource));
            });

        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = localizationSettings.SupportedCultures
                .Select(c => new CultureInfo(c))
                .ToList();

            options.DefaultRequestCulture = new RequestCulture(localizationSettings.DefaultCulture);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
    }
}
