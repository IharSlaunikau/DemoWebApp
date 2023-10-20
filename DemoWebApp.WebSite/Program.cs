using DemoWebApp.WebSite.Settings;
using Microsoft.Extensions.Options;
using Serilog;

namespace DemoWebApp.WebSite;

public static class Program
{
    public static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        try
        {
            var appLocation = AppContext.BaseDirectory;

            Log.Information("Starting application at {AppLocation}...", appLocation);

            var host = CreateHostBuilder(args).Build();

            var appSettings = host.Services.GetRequiredService<IOptions<AppSettings>>()?.Value;

            if (appSettings != null)
            {
                Log.Information("Current configuration: NorthwindConnection = {NorthwindConnection}", appSettings.NorthwindConnection);
            }

            var productSettings = host.Services.GetRequiredService<IOptions<ProductSettings>>()?.Value;

            if (productSettings != null)
            {
                Log.Information("Product settings: MaxAmount = {MaxAmount}", productSettings.MaxAmount);
            }

            host.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application startup failed");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .UseSerilog();
}
