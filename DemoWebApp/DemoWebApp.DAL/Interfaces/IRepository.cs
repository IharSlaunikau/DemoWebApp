using DemoWebApp.DAL.Models;

namespace DemoWebApp.DAL.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TEntity> GetByIdAsync(int id);

    Task<TEntity> AddAsync(TEntity entity);

    Task<TEntity> UpdateFieldsAsync(TEntity updatedEntity, TEntity previousEntity);
}
