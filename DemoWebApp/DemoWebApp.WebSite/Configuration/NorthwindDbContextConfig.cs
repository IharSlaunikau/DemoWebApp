using DemoWebApp.DAL;
using DemoWebApp.WebSite.Settings;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApp.WebSite.Configuration;

public static class NorthwindDbContextConfig
{
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        var northwindSettings = configuration.GetSection("NorthwindSettings").Get<NorthwindSettings>();

        var connectionString = northwindSettings?.ConnectionString;
        if (String.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string in NorthwindSettings is null or empty.");
        }

        services.AddDbContext<NorthwindDbContext>(options => options.UseSqlServer(connectionString));
    }
}
