using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using DemoWebApp.DAL;
using DemoWebApp.DAL.Interfaces;
using DemoWebApp.DAL.Models;
using DemoWebApp.DAL.Repositories;
using DemoWebApp.WebSite.Middleware;
using DemoWebApp.WebSite.Models;
using DemoWebApp.WebSite.Settings;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DemoWebApp.WebSite;

[ExcludeFromCodeCoverage]
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var northwindSettings = Configuration.GetSection("NorthwindSettings").Get<NorthwindSettings>();

        var connectionString = northwindSettings?.ConnectionString;
        if (String.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string in NorthwindSettings is null or empty.");
        }

        services.AddDbContext<NorthwindDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IPropertyUpdater<Category>, PropertyUpdater<Category>>();
        services.AddScoped<IPropertyUpdater<Product>, PropertyUpdater<Product>>();

        services.AddScoped<CustomExceptionHandler>();

        MappingConfig.Configure();

        var localizationSettings = Configuration.GetSection("LocalizationSettings").Get<LocalizationSettings>();
        services.Configure<LocalizationSettings>(Configuration.GetSection("LocalizationSettings"));

        services
            .AddLocalization(options => { options.ResourcesPath = "Resources"; })
            .AddControllersWithViews()
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
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}