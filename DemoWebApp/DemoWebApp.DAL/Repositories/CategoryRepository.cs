using DemoWebApp.DAL.Interfaces;
using DemoWebApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApp.DAL.Repositories;
public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(NorthwindDbContext dbContext, IPropertyUpdater<Category> updater)
        : base(dbContext, updater)
    { }

    public override async Task<Category> GetByIdAsync(int id) =>
        await Entities.AsNoTracking().FirstOrDefaultAsync(category => category.Id == id);
}
