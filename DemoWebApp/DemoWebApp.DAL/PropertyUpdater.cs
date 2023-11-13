using System.Reflection;
using DemoWebApp.DAL.Interfaces;
using DemoWebApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApp.DAL;

public class PropertyUpdater<TEntity> : IPropertyUpdater<TEntity> where TEntity : BaseEntity
{
    public void UpdatePropertyValues(TEntity updatedEntity, TEntity previousEntity, DbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(updatedEntity);
        ArgumentNullException.ThrowIfNull(previousEntity);
        ArgumentNullException.ThrowIfNull(dbContext);

        PropertyInfo[] properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (PropertyInfo property in properties)
        {
            if (dbContext.Model.FindEntityType(typeof(TEntity)).FindNavigation(property.Name) != null)
            {
                continue;
            }

            var updatedValue = property.GetValue(updatedEntity);
            var previousValue = property.GetValue(previousEntity);

            if (updatedValue != null && !updatedValue.Equals(previousValue))
            {
                property.SetValue(previousEntity, updatedValue);
                dbContext.Entry(previousEntity).Property(property.Name).IsModified = true;
            }
        }
    }
}
