using DemoWebApp.DAL;
using DemoWebApp.DAL.Interfaces;
using DemoWebApp.DAL.Repositories;
using DemoWebApp.WebSite.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using DemoWebApp.WebSite.Models;

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
        services.AddDbContext<NorthwindDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("NorthwindDatabase")));

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

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
            app.UseExceptionHandler("/Home/Error");
        }
        app.UseStaticFiles();

        var localizationOptions = Configuration.GetSection("Localization");
        var defaultCulture = localizationOptions.GetValue<string>("DefaultCulture");
        var supportedCultures = localizationOptions.GetSection("SupportedCultures").Get<string[]>()
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
