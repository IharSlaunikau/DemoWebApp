using Microsoft.EntityFrameworkCore;

namespace DemoWebApp.DAL;

public class NorthwindDbContext : DbContext
{
    public NorthwindDbContext(DbContextOptions<NorthwindDbContext> options)
        : base(options)
    { }
}
