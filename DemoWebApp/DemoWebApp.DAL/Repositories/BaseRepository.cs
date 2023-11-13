using DemoWebApp.DAL.Interfaces;
using DemoWebApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApp.DAL.Repositories;

public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly IPropertyUpdater<TEntity> _updater;

    protected NorthwindDbContext DbContext { get; }
    protected DbSet<TEntity> Entities { get; }

    protected BaseRepository
        (
            NorthwindDbContext dbContext,
            IPropertyUpdater<TEntity> updater
        )
    {
        _updater = updater;
        ArgumentNullException.ThrowIfNull(dbContext);

        DbContext = dbContext;
        Entities = dbContext.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync() =>
        await Entities.ToListAsync();

    public virtual async Task<TEntity> GetByIdAsync(int id) =>
        await Entities.FindAsync(id);

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await DbContext.Set<TEntity>().AddAsync(entity);

        await DbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<TEntity> UpdateFieldsAsync(TEntity updatedEntity, TEntity previousEntity)
    {
        ArgumentNullException.ThrowIfNull(updatedEntity);
        ArgumentNullException.ThrowIfNull(previousEntity);

        _updater.UpdatePropertyValues(updatedEntity, previousEntity, DbContext);

        await DbContext.SaveChangesAsync();

        return updatedEntity;
    }
}
