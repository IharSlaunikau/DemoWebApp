using DemoWebApp.DAL.Interfaces;
using DemoWebApp.DAL.Models;
using DemoWebApp.WebSite.Controllers;
using DemoWebApp.WebSite.Settings;
using DemoWebApp.WebSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Moq;

namespace DemoWebApp.WebSite.Tests.Controllers;

public class ProductControllerTests
{
    private const string CategoriesViewDataKey = "Categories";

    private Mock<IProductRepository> _productRepositoryMock;
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private IOptions<ProductSettings> _productSettings;
    private ProductController _controller;

    [SetUp]
    public void SetUp()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();

        _productSettings = Options.Create(new ProductSettings { MaxAmount = 10 });

        _controller = new ProductController
        (
            _productRepositoryMock.Object,
            _productSettings,
            _categoryRepositoryMock.Object
        );
    }

    [Test]
    public async Task Index_ReturnsViewResult_WithProductViewModels()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = 1, ProductName = "Product 1", CategoryId = 1 },
            new() { Id = 2, ProductName = "Product 2", CategoryId = 1 },
        };

        _productRepositoryMock
            .Setup(repo => repo.GetLimitedProducts(It.IsAny<int>()))
            .Returns(Task.FromResult(products));

        // Act
        var result = await _controller.Index();

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);

        var viewResult = (ViewResult)result;

        Assert.IsInstanceOf<IEnumerable<ProductViewModel>>(viewResult.Model);
        Assert.AreEqual(_productSettings.Value.MaxAmount, viewResult.ViewData["MaxAmount"]);
    }

    [Test]
    public async Task Create_GET_ReturnsViewResult_WithCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new() { Id = 1, CategoryName = "Category 1" },
            new() { Id = 2, CategoryName = "Category 2" },
        };

        _categoryRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(categories);

        // Act
        var result = await _controller.Create();

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);

        var viewResult = (ViewResult)result;

        Assert.NotNull(viewResult.ViewData[CategoriesViewDataKey]);
        Assert.IsInstanceOf<IEnumerable<SelectListItem>>(viewResult.ViewData[CategoriesViewDataKey]);
    }

    [Test]
    public async Task Create_POST_ModelStateNotValid_ReturnsCreateView()
    {
        // Arrange
        _controller.ModelState.AddModelError("error", "test error");

        var productViewModel = new ProductViewModel
        {
            Id = 1,
            ProductName = "Product 1",
            CategoryId = 1
        };

        // Act
        var result = await _controller.Create(productViewModel);

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);

        var viewResult = (ViewResult)result;

        Assert.AreEqual("Create", viewResult.ViewName);
        Assert.IsInstanceOf<ProductViewModel>(viewResult.Model);
    }

    [Test]
    public async Task Create_POST_Success_RedirectsToIndex()
    {
        // Arrange
        var productViewModel = new ProductViewModel
        {
            Id = 1,
            ProductName = "Product 1",
            CategoryId = 1
        };

        // Act
        var result = await _controller.Create(productViewModel);

        // Assert
        Assert.IsInstanceOf<RedirectToActionResult>(result);

        var redirectToResult = (RedirectToActionResult)result;

        Assert.AreEqual("Index", redirectToResult.ActionName);
    }

    [Test]
    public async Task Edit_GET_ProductNotFound_ReturnsNotFound()
    {
        // Arrange
        _productRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Product)null);

        // Act
        var result = await _controller.Edit(id: 1);

        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task Edit_GET_ProductFound_ReturnsEditView()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            ProductName = "Product 1",
            CategoryId = 1
        };

        _productRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(product);

        var categories = new List<Category>
        {
            new() { Id = 1, CategoryName = "Category 1" },
            new() { Id = 2, CategoryName = "Category 2" },
        };

        _categoryRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(categories);

        // Act
        var result = await _controller.Edit(id: 1);

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);

        var viewResult = (ViewResult)result;

        Assert.IsInstanceOf<ProductViewModel>(viewResult.Model);
        Assert.NotNull(viewResult.ViewData[CategoriesViewDataKey]);
        Assert.IsInstanceOf<IEnumerable<SelectListItem>>(viewResult.ViewData[CategoriesViewDataKey]);
    }

    [Test]
    public async Task Edit_POST_ModelStateNotValid_ReturnsEditView()
    {
        // Arrange
        _controller.ModelState.AddModelError("error", "test error");

        var productViewModel = new ProductViewModel
        {
            Id = 1,
            ProductName = "Product 1",
            CategoryId = 1
        };

        // Act
        var result = await _controller.Edit(id: 1, productViewModel);

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);

        var viewResult = (ViewResult)result;

        Assert.AreEqual("Edit", viewResult.ViewName);
        Assert.IsInstanceOf<ProductViewModel>(viewResult.Model);
    }

    [Test]
    public async Task Edit_POST_ProductNotFound_ReturnsNotFound()
    {
        // Arrange
        _productRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Product)null);

        var productViewModel = new ProductViewModel
        {
            Id = 1,
            ProductName = "Product 1",
            CategoryId = 1
        };

        // Act
        var result = await _controller.Edit(id: 1, productViewModel);

        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task Edit_POST_Success_RedirectsToIndex()
    {
        // Arrange
        var product = new Product { Id = 1, ProductName = "Product 1", CategoryId = 1 };

        _productRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(product);

        _productRepositoryMock
            .Setup(repo =>
                repo.UpdateFieldsAsync(
                    It.IsAny<Product>(),
                    It.IsAny<Product>()))
            .Verifiable();

        var productViewModel = new ProductViewModel
        {
            Id = 1,
            ProductName = "Product 1",
            CategoryId = 1
        };

        // Act
        var result = await _controller.Edit(id: 1, productViewModel);

        // Assert
        Assert.IsInstanceOf<RedirectToActionResult>(result);

        var redirectToResult = (RedirectToActionResult)result;

        Assert.AreEqual("Index", redirectToResult.ActionName);
    }
}
