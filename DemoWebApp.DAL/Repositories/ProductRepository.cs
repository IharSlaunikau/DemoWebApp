using DemoWebApp.DAL.Interfaces;
using DemoWebApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApp.DAL.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    private const int ShowAllProducts = 0;

    public ProductRepository(NorthwindDbContext dbContext)
        : base(dbContext)
    { }

    public async Task<List<Product>> GetLimitedProducts(int maxAmount)
    {
        var products = maxAmount == ShowAllProducts
            ? await DbContext.Products.Include(p => p.Category).ToListAsync()
            : await DbContext.Products.Include(p => p.Category).Take(maxAmount).ToListAsync();

        return products;
    }
}
