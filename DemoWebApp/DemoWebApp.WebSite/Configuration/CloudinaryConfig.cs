using CloudinaryDotNet;
using DemoWebApp.WebSite.Settings;
using Microsoft.Extensions.Options;

namespace DemoWebApp.WebSite.Configuration;

public static class CloudinaryConfig
{
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(provider =>
        {
            var cloudinarySettings = provider.GetRequiredService<IOptions<CloudinarySettings>>().Value;
            var myAccount = new Account(cloudinarySettings.CloudName, cloudinarySettings.ApiKey, cloudinarySettings.ApiSecret);
            return new Cloudinary(myAccount);
        });

        services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
    }
}
