using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DemoWebApp.DAL;

public class NorthwindDbContextFactory : IDesignTimeDbContextFactory<NorthwindDbContext>
{
    public NorthwindDbContext CreateDbContext(string[] args)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var solutionDirectory = Directory.GetParent(currentDirectory)!.FullName;
        var webProjectDirectory = Path.Combine(solutionDirectory, "DemoWebApp.WebSite");

        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(webProjectDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetSection("NorthwindSettings")["ConnectionString"];

        var dbContextOptionsBuilder = new DbContextOptionsBuilder<NorthwindDbContext>();
        dbContextOptionsBuilder.UseSqlServer(connectionString);

        return new NorthwindDbContext(dbContextOptionsBuilder.Options);
    }
}
