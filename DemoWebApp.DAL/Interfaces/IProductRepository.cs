using DemoWebApp.DAL.Models;

namespace DemoWebApp.DAL.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();

    Task<List<Product>> GetLimitedProducts(int maxAmount);

    Task<Product> GetByIdAsync(int id);

    Task<Product> AddAsync(Product product);

    Task<Product> UpdateFieldsAsync(Product updatedEntity, Product previousEntity);
}
