using DemoWebApp.DAL.Interfaces;
using DemoWebApp.DAL.Models;
using DemoWebApp.WebSite.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using CloudinaryDotNet;
using DemoWebApp.WebSite.ViewModels;

namespace DemoWebApp.WebSite.Tests.Controllers;

public class CategoryControllerTests
{
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private CategoryController _controller;
    private Cloudinary _cloudinary;

    [SetUp]
    public void SetUp()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();

        var config = new Dictionary<string, string>();
        var cloudinaryConfiguration = new CloudinaryDotNet.Account
        {
            ApiKey = "test",
            ApiSecret = "test",
            Cloud = "test"
        };

        _cloudinary = new Cloudinary(cloudinaryConfiguration);

        _controller = new CategoryController(_categoryRepositoryMock.Object, _cloudinary);
    }

    [Test]
    public async Task Index_ReturnsViewResult_WithCategoryViewModels()
    {
        // Arrange
        IList<Category> categories = new List<Category>
        {
            new() { Id = 1, CategoryName = "Category 1" },
            new() { Id = 2, CategoryName = "Category 2" },
        };

        _categoryRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(categories);

        // Act
        var result = await _controller.Index();

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);

        var viewResult = (ViewResult)result;

        Assert.IsInstanceOf<IEnumerable<CategoryViewModel>>(viewResult.Model);
    }

    [Test]
    public async Task Index_NoCategories_ReturnsViewResult_WithEmptyCategoryViewModels()
    {
        // Arrange
        _categoryRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Category>());

        // Act
        var result = await _controller.Index();

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);

        var viewResult = (ViewResult)result;

        Assert.IsInstanceOf<IEnumerable<CategoryViewModel>>(viewResult.Model);

        var model = (IEnumerable<CategoryViewModel>)viewResult.Model;

        Assert.IsFalse(model.Any());
    }
}
