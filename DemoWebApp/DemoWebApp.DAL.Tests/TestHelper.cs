using System.Collections;
using System.Text.Json;
using DemoWebApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

namespace DemoWebApp.DAL.Tests;

public static class TestHelper
{
    public static DbSet<TEntity> CreateDbSetMock<TEntity>(IQueryable<TEntity> data) where TEntity : BaseEntity
    {
        int primaryKeyIndex = 0;

        var mockSet = new Mock<DbSet<TEntity>>();

        var asyncEnumerable = new TestAsyncEnumerable<TEntity>(data);

        mockSet
            .As<IAsyncEnumerable<TEntity>>()
            .Setup(enumerable => enumerable.GetAsyncEnumerator(default))
            .Returns(asyncEnumerable.GetAsyncEnumerator())
            .Verifiable();

        mockSet
            .As<IQueryable<TEntity>>()
            .Setup(queryable => queryable.Provider)
            .Returns(data.Provider)
            .Verifiable();

        mockSet
            .As<IQueryable<TEntity>>()
            .Setup(queryable => queryable.ElementType)
            .Returns(data.ElementType)
            .Verifiable();

        mockSet
            .As<IQueryable<TEntity>>()
            .Setup(queryable => queryable.Expression)
            .Returns(data.Expression)
            .Verifiable();

        mockSet
            .As<IQueryable<TEntity>>()
            .Setup(queryable => queryable.GetEnumerator())
            .Returns(data.GetEnumerator())
            .Verifiable();

        mockSet
            .Setup(mockDbSet => mockDbSet.FindAsync(It.IsAny<object[]>()))
            .Returns((object[] keyValues) =>
                ValueTask.FromResult(data.FirstOrDefault(entity => entity.Id == (int)keyValues[primaryKeyIndex])))
            .Verifiable();

        mockSet
            .Setup(set => set.AddAsync(It.IsAny<TEntity>(), It.IsAny<CancellationToken>()))
            .Returns((TEntity entity, CancellationToken token) =>
            {
                data = data.Append(entity);
                return new ValueTask<EntityEntry<TEntity>>(Task.FromResult((EntityEntry<TEntity>)null));
            });

        return mockSet.Object;
    }

    public static IEnumerable<TEntity> CreateCollectionEntity<TEntity>() where TEntity : BaseEntity, new()
    {
        return typeof(TEntity) switch
        {
            var type when type == typeof(Product) => CreateSampleProducts() as IEnumerable<TEntity>,
            var type when type == typeof(Category) => CreateSampleCategories() as IEnumerable<TEntity>
        };
    }

    public static TEntity CreateNewEntity<TEntity>() where TEntity : BaseEntity, new()
    {
        return typeof(TEntity) switch
        {
            var type when type == typeof(Product) => (TEntity)(BaseEntity)CreateSampleProduct(),
            var type when type == typeof(Category) => (TEntity)(BaseEntity)CreateSampleCategory()
        };
    }

    public static Category GetCategory()
    {
        var pages = new HashSet<byte> { 1, 2, 3 };
        var products = new List<Product>
        {
            new() {Id = 1, ProductName = "Test Product 1"},
            new() {Id = 2, ProductName = "Test Product 2"},
        };

        var category = new Category
        {
            Id = 1,
            Description = "Test Description",
            CategoryName = "Test Category"
        };

        foreach (var page in pages)
        {
            ((IList)category.Picture).Add(page);
        }

        foreach (var product in products)
        {
            category.Products.Add(product);
        }

        return category;
    }

    private static Category CreateSampleCategory()
    {
        return new Category
        {
            Id = 4,
            CategoryName = "Sample Category",
            Description = "Sample Description",
        };
    }

    private static Product CreateSampleProduct()
    {
        return new Product
        {
            Id = 4,
            ProductName = "Sample Product",
            CategoryId = 4,
            Category = CreateSampleCategory()
        };
    }

    private static IEnumerable<Product> CreateSampleProducts()
    {
        return new List<Product>
        {
            new Product { Id = 1, ProductName = "Product 1", CategoryId = 4, Category = CreateSampleCategory() },
            new Product { Id = 2, ProductName = "Product 2", CategoryId = 4, Category = CreateSampleCategory() },
            new Product { Id = 3, ProductName = "Product 3", CategoryId = 4, Category = CreateSampleCategory() }
        };
    }

    private static IEnumerable<Category> CreateSampleCategories()
    {
        return new List<Category>
        {
            new Category { Id = 1, CategoryName = "Category 1", Description = "Test Category 1" },
            new Category { Id = 2, CategoryName = "Category 2", Description = "Test Category 2" },
            new Category { Id = 3, CategoryName = "Category 3", Description = "Test Category 3" },
        };
    }

    public static string ToJson(object value) => JsonSerializer.Serialize(value);
}

public class TestAsyncEnumerable<TEntity> : IAsyncEnumerable<TEntity>
{
    private readonly IQueryable<TEntity> _queryable;

    public TestAsyncEnumerable(IQueryable<TEntity> queryable)
    {
        _queryable = queryable;
    }

    public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
            new TestAsyncEnumerator<TEntity>(_queryable.GetEnumerator());
}

public class TestAsyncEnumerator<TEntity> : IAsyncEnumerator<TEntity>
{
    private readonly IEnumerator<TEntity> _enumerator;

    public TestAsyncEnumerator(IEnumerator<TEntity> enumerator)
    {
        _enumerator = enumerator;
    }

    public ValueTask<bool> MoveNextAsync() =>
        new(_enumerator.MoveNext());

    public TEntity Current =>
        _enumerator.Current;

    public ValueTask DisposeAsync()
    {
        _enumerator.Dispose();
        return default;
    }
}
