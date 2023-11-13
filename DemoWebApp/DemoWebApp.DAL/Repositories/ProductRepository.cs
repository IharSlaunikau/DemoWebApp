using DemoWebApp.DAL.Interfaces;
using DemoWebApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApp.DAL.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    private const int ShowAllProducts = 0;

    public ProductRepository(NorthwindDbContext dbContext, IPropertyUpdater<Product> updater)
    : base(dbContext, updater)
    { }

    public override async Task<Product> GetByIdAsync(int id) =>
        await Entities.Include(product => product.Category).FirstOrDefaultAsync(product => product.Id == id);

    public async Task<List<Product>> GetLimitedProducts(int maxAmount)
    {
        var products = maxAmount == ShowAllProducts
            ? await DbContext.Products.Include(product => product.Category).ToListAsync()
            : await DbContext.Products.Include(product => product.Category).Take(maxAmount).ToListAsync();

        return products;
    }
}
