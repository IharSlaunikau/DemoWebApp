using System.Diagnostics.CodeAnalysis;
using DemoWebApp.WebSite.Configuration;
using DemoWebApp.WebSite.Middleware;
using DemoWebApp.WebSite.Settings;
using Microsoft.Extensions.Options;

namespace DemoWebApp.WebSite;

[ExcludeFromCodeCoverage]
public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        ApplicationServicesConfig.Configure(services);
        LocalizationConfig.Configure(services, Configuration);
        LogActionFilterConfig.Configure(services, Configuration);
        NorthwindDbContextConfig.Configure(services, Configuration);
        CloudinaryConfig.Configure(services, Configuration);

        services.Configure<LogSettings>(Configuration.GetSection(nameof(LogSettings)));
        services.Configure<ProductSettings>(Configuration.GetSection(nameof(ProductSettings)));

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler(builder => builder.UseMiddleware<CustomExceptionHandler>());
        }

        app.UseStaticFiles();

        var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value;
        app.UseRequestLocalization(localizationOptions);

        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "imageRoute",
                pattern: "images/{image_id}",
                defaults: new { controller = "Category", action = "GetImage" });

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}
