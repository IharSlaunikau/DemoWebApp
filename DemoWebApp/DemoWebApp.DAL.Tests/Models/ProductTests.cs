using DemoWebApp.DAL.Models;
using NUnit.Framework;

namespace DemoWebApp.DAL.Tests.Models;

internal class ProductTests
{
    [Test]
    public void Clone_WhenCalled_CreatesCorrectClone()
    {
        // Arrange
        var expectedProduct = new Product
        {
            Id = 1,
            ProductName = "Test Product",
            SupplierId = 2,
            CategoryId = 3,
            QuantityPerUnit = "10",
            UnitPrice = 10.0m,
            UnitsInStock = 10,
            UnitsOnOrder = 4,
            ReorderLevel = 5,
            Discontinued = false,
            Category = new Category { Id = 3, CategoryName = "TestCategory" }
        };

        // Act
        var actualProduct = expectedProduct.Clone();

        // Assert
        Assert.IsNotNull(actualProduct);
        Assert.AreEqual(TestHelper.ToJson(expectedProduct), TestHelper.ToJson(actualProduct));
    }
}
