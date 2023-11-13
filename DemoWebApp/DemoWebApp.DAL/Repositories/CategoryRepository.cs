using DemoWebApp.DAL.Interfaces;
using DemoWebApp.DAL.Models;

namespace DemoWebApp.DAL.Repositories;
public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(NorthwindDbContext dbContext, IPropertyUpdater<Category> updater)
        : base(dbContext, updater)
    { }
}
