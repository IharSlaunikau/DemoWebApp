namespace DemoWebApp.WebSite;

public static class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.ConfigureLogging((hostingContext, logging) =>
                {
                    var logPath = Path.Combine(hostingContext.Configuration.GetValue<string>("Logging:File:Path"),
                        hostingContext.Configuration.GetValue<string>("Logging:File:Name"));
                    logging.AddFile(logPath);
                });
            });
}
