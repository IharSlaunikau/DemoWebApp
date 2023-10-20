using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApp.DAL.Repositories;

public abstract class BaseRepository<TEntity> where TEntity : class
{
    protected NorthwindDbContext DbContext { get; }
    protected DbSet<TEntity> Entities { get; }

    protected BaseRepository(NorthwindDbContext dbContext)
    {
        DbContext = dbContext;
        Entities = dbContext.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync() => await Entities.ToListAsync();

    public async Task<TEntity> GetByIdAsync(int id) => await Entities.FindAsync(id);

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await DbContext.Set<TEntity>().AddAsync(entity);

        await DbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<TEntity> UpdateFieldsAsync(TEntity updatedEntity, TEntity previousEntity)
    {
        PropertyInfo[] properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var dbEntityEntry = DbContext.Entry(updatedEntity);

        foreach (PropertyInfo property in properties)
        {
            var updatedValue = property.GetValue(updatedEntity);
            var previousValue = property.GetValue(previousEntity);

            if (updatedValue != null && !updatedValue.Equals(previousValue))
            {
                dbEntityEntry.Property(property.Name).IsModified = true;
            }
        }

        await DbContext.SaveChangesAsync();

        return updatedEntity;
    }
}
