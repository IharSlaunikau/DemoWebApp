using DemoWebApp.DAL;
using DemoWebApp.DAL.Interfaces;
using DemoWebApp.DAL.Models;
using DemoWebApp.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace DemoWebApp.DAL.Tests.Repositories
{
    [TestFixture(typeof(Category))]
    [TestFixture(typeof(Product))]
    public class BaseRepositoryTests<TEntity> where TEntity : BaseEntity, new()
    {
        private DbSet<TEntity> _mockSet;
        private TestRepository<TEntity> _repository;

        private Mock<NorthwindDbContext> _mockDbContext;
        private Mock<IPropertyUpdater<TEntity>> _mockUpdater;

        [SetUp]
        public void Setup()
        {
            var sampleData = TestHelper.CreateCollectionEntity<TEntity>().AsQueryable();

            _mockSet = TestHelper.CreateDbSetMock(sampleData);

            var dbContextOptions = new DbContextOptions<NorthwindDbContext>();
            _mockDbContext = new Mock<NorthwindDbContext>(dbContextOptions);

            _mockUpdater = new Mock<IPropertyUpdater<TEntity>>();

            _mockDbContext
                .Setup(expression: c => c.Set<TEntity>())
                .Returns(_mockSet)
                .Verifiable();

            _repository = new TestRepository<TEntity>(_mockDbContext.Object, _mockUpdater.Object);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllEntities()
        {
            // Arrange
            var expectedEntity = TestHelper.CreateCollectionEntity<TEntity>();

            // Act
            var actualEntity = await _repository.GetAllAsync();

            // Assert
            Assert.That(TestHelper.ToJson(actualEntity), Is.EqualTo(TestHelper.ToJson(expectedEntity)));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsEntityWithGivenId()
        {
            // Arrange
            const int id = 1;
            var expectedEntities = TestHelper.CreateCollectionEntity<TEntity>().FirstOrDefault(entity => entity.Id == id);

            // Act
            var actualEntities = await _repository.GetByIdAsync(id);

            // Assert
            Assert.That(TestHelper.ToJson(actualEntities), Is.EqualTo(TestHelper.ToJson(expectedEntities)));
        }

        [Test]
        public async Task AddAsync_WhenCalled_EntityIsAdded()
        {
            // Arrange
            var expectedEntity = TestHelper.CreateNewEntity<TEntity>();

            // Act
            var actualEntity = await _repository.AddAsync(expectedEntity);

            // Assert
            Assert.That(TestHelper.ToJson(actualEntity), Is.EqualTo(TestHelper.ToJson(expectedEntity)));
        }

        [Test]
        public async Task UpdateFieldsAsync_WhenCalled_EntityIsUpdated()
        {
            // Arrange
            var previousEntity = TestHelper.CreateNewEntity<TEntity>();
            var updatedEntity = TestHelper.CreateNewEntity<TEntity>();

            // Act
            var actualEntity = await _repository.UpdateFieldsAsync(updatedEntity, previousEntity);

            // Assert
            Assert.That(TestHelper.ToJson(actualEntity), Is.EqualTo(TestHelper.ToJson(previousEntity)));
        }

        [Test]
        public void UpdateFieldsAsync_WhenCalledWithNullPreviousEntity_ThrowsArgumentNullException()
        {
            // Arrange
            var expectedEntity = TestHelper.CreateNewEntity<TEntity>();

            // Assert
            Assert.That(async () => await _repository.UpdateFieldsAsync(expectedEntity, previousEntity: null),
                Throws.ArgumentNullException);
        }

        [Test]
        public void UpdateFieldsAsync_WhenCalledWithNullUpdatedEntity_ThrowsArgumentNullException()
        {
            // Arrange
            var expectedEntity = TestHelper.CreateNewEntity<TEntity>();

            // Assert
            Assert.That(async () => await _repository.UpdateFieldsAsync(updatedEntity: null, expectedEntity),
                Throws.ArgumentNullException);
        }

        [Test]
        public void AddAsync_WhenCalledWithNullTEntity_ThrowsArgumentNullException()
        {
            // Assert
            Assert.That(async () => await _repository.AddAsync(entity: null), Throws.ArgumentNullException);
        }
    }
}


public class TestRepository<TEntity> : BaseRepository<TEntity> where TEntity : BaseEntity
{
    public TestRepository(NorthwindDbContext dbContext, IPropertyUpdater<TEntity> updater) : base(dbContext, updater)
    { }
}
