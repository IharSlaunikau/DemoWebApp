using DemoWebApp.DAL;
using DemoWebApp.DAL.Interfaces;
using DemoWebApp.DAL.Models;
using DemoWebApp.DAL.Repositories;
using DemoWebApp.WebSite.Filters;
using DemoWebApp.WebSite.Middleware;

namespace DemoWebApp.WebSite.Configuration;

public static class ApplicationServicesConfig
{
    public static void Configure(IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IPropertyUpdater<Category>, PropertyUpdater<Category>>();
        services.AddScoped<IPropertyUpdater<Product>, PropertyUpdater<Product>>();

        services.AddScoped<CustomExceptionHandler>();
        services.AddScoped<LogActionFilter>();
    }
}
