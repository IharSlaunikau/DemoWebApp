using DemoWebApp.DAL.Models;

namespace DemoWebApp.DAL.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetLimitedProducts(int maxAmount);
}
