using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DemoWebApp.DAL;

public class NorthwindDbContextFactory : IDesignTimeDbContextFactory<NorthwindDbContext>
{
    public NorthwindDbContext CreateDbContext(string[] args)
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string solutionDirectory = Directory.GetParent(currentDirectory).FullName;
        string webProjectDirectory = Path.Combine(solutionDirectory, "DemoWebApp.WebSite");

        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(webProjectDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        Console.WriteLine("Current Directory: " + currentDirectory);
        Console.WriteLine("Solution Directory: " + solutionDirectory);
        Console.WriteLine("Web Project Directory: " + webProjectDirectory);

        var connectionString = configuration.GetSection("NorthwindSettings")["ConnectionString"];

        var dbContextOptionsBuilder = new DbContextOptionsBuilder<NorthwindDbContext>();
        dbContextOptionsBuilder.UseSqlServer(connectionString);

        return new NorthwindDbContext(dbContextOptionsBuilder.Options);
    }
}
