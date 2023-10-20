using DemoWebApp.DAL.Models;

namespace DemoWebApp.DAL.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();

    Task<Category> GetByIdAsync(int id);

    Task<Category> AddAsync(Category category);

    Task<Category> UpdateFieldsAsync(Category updatedEntity, Category previousEntity);
}
