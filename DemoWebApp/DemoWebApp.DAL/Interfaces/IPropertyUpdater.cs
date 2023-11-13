using DemoWebApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApp.DAL.Interfaces;

public interface IPropertyUpdater<in TEntity> where TEntity : BaseEntity
{
    void UpdatePropertyValues(TEntity updatedEntity, TEntity previousEntity, DbContext dbContext);
}
