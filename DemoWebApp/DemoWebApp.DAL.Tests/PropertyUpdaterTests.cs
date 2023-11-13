using DemoWebApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DemoWebApp.DAL.Tests;

public class PropertyUpdaterTests
{
    [Test]
    public void UpdatePropertyValues_UpdatesChangedProperties()
    {
        const int productId = 1;

        // Arrange
        var previousEntity = new Product { Id = productId, ProductName = "OldName", UnitPrice = 10m };
        var updatedEntity = new Product { Id = productId, ProductName = "NewName", UnitPrice = 15m };

        var options = new DbContextOptionsBuilder<NorthwindDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using var dbContext = new NorthwindDbContext(options);

        dbContext.Add(previousEntity);
        dbContext.SaveChanges();

        // Act
        new PropertyUpdater<Product>().UpdatePropertyValues(updatedEntity, dbContext.Set<Product>().Find(productId), dbContext);
        dbContext.SaveChanges();

        // Assert
        var updatedProduct = dbContext.Set<Product>().FirstOrDefault(p => p.Id == productId);

        Assert.IsNotNull(updatedProduct);
        Assert.AreEqual(updatedEntity.ProductName, updatedProduct.ProductName);
        Assert.AreEqual(updatedEntity.UnitPrice, updatedProduct.UnitPrice);
    }
}
