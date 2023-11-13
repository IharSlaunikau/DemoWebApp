using DemoWebApp.DAL.Interfaces;
using DemoWebApp.DAL.Models;
using DemoWebApp.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace DemoWebApp.DAL.Tests.Repositories;

public class ProductRepositoryTests
{
    private NorthwindDbContext _dbContext;
    private Mock<IPropertyUpdater<Product>> _propertyUpdaterMock;
    private ProductRepository _repository;
    private List<Product> _testData;

    [SetUp]
    public void SetUp()
    {
        _testData = TestHelper.CreateCollectionEntity<Product>().ToList();
        var options = new DbContextOptionsBuilder<NorthwindDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase" + Guid.NewGuid())
            .Options;

        _dbContext = new NorthwindDbContext(options);

        // Temporarily store categories
        var categories = _testData.Select(p => p.Category).ToList();

        // Remove Category from Product to avoid tracking issue
        foreach (var product in _testData)
        {
            product.Category = null;
        }

        _dbContext.Products.AddRange(_testData);
        _dbContext.SaveChanges();

        // Reassign stored categories back to Product entities
        for (var i = 0; i < _testData.Count; i++)
        {
            _testData[i].Category = categories[i];
        }

        _propertyUpdaterMock = new Mock<IPropertyUpdater<Product>>();
        _repository = new ProductRepository(_dbContext, _propertyUpdaterMock.Object);
    }

    [Test]
    public async Task GetByIdAsync_ReturnsProductWithCategory()
    {
        // Arrange
        const int expectedProductId = 1;
        var expectedProduct = _testData.First(p => p.Id == expectedProductId);

        // Act
        var actualProduct = await _repository.GetByIdAsync(expectedProductId);

        // Assert
        Assert.IsNotNull(actualProduct);
        Assert.That(TestHelper.ToJson(expectedProduct), Is.EqualTo(TestHelper.ToJson(actualProduct)));
    }

    [Test]
    public async Task GetLimitedProducts_ReturnsLimitedProductsAndCategories()
    {
        // Arrange
        const int limit = 2;
        var expectedProducts = _testData.Take(limit).ToList();

        // Act
        var actualProducts = await _repository.GetLimitedProducts(limit);

        // Assert
        Assert.IsNotNull(actualProducts);
        Assert.That(TestHelper.ToJson(expectedProducts), Is.EqualTo(TestHelper.ToJson(actualProducts)));
    }
}
