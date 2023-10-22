using System.Globalization;
using DemoWebApp.DAL;
using DemoWebApp.DAL.Interfaces;
using DemoWebApp.DAL.Repositories;
using DemoWebApp.WebSite.Middleware;
using DemoWebApp.WebSite.Models;
using DemoWebApp.WebSite.Settings;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApp.WebSite;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = Configuration.GetConnectionString("NorthwindDatabase");
        Console.WriteLine($"ConnectionString: {connectionString}"); // Добавить эту строку, чтобы вывести строку подключения

        services.AddDbContext<NorthwindDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<CustomExceptionHandler>();

        MappingConfig.Configure();

        services.AddControllersWithViews()
            .AddViewLocalization()
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResource));
            });

        services.Configure<ProductSettings>(Configuration.GetSection("ProductSettings"));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var errorHandler = errorApp.ApplicationServices.GetRequiredService<CustomExceptionHandler>();
                    await errorHandler.Handle(context);
                });
            });
        }

        app.UseStaticFiles();

        var localizationOptions = Configuration.GetSection("Localization");

        var defaultCulture = localizationOptions.GetValue<string>("DefaultCulture");

        if (String.IsNullOrWhiteSpace(defaultCulture))
        {
            defaultCulture = "en-US";
        }

        var supportedCulturesArray = localizationOptions.GetSection("SupportedCultures").Get<string[]>() ?? new[] { defaultCulture };

        var validCultures = supportedCulturesArray.Where(c => !String.IsNullOrWhiteSpace(c));

        var supportedCultures = validCultures
            .Select(c => new CultureInfo(c)).ToArray();

        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(defaultCulture),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}
