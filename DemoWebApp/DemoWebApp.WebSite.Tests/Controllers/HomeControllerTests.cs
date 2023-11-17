using DemoWebApp.WebSite.Controllers;
using DemoWebApp.WebSite.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace DemoWebApp.WebSite.Tests.Controllers;

public class HomeControllerTests
{
    private HomeController _controller;
    private Mock<ILogger<HomeController>> _logger;
    private Mock<HttpContext> _httpContextMock;

    [SetUp]
    public void SetUp()
    {
        _logger = new Mock<ILogger<HomeController>>();
        _httpContextMock = new Mock<HttpContext>();

        _controller = new HomeController(_logger.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = _httpContextMock.Object
            }
        };
    }

    [Test]
    public void Index_ReturnsViewResult()
    {
        // Act
        var result = _controller.Index();

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
    }

    [Test]
    public void Privacy_ReturnsViewResult()
    {
        // Act
        var result = _controller.Privacy();

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
    }

    [Test]
    public void Error_ReturnsViewResult_WithErrorViewModel()
    {
        // Arrange
        _httpContextMock
            .SetupGet(c => c.TraceIdentifier)
            .Returns("TestTraceIdentifier")
            .Verifiable();

        // Act
        var result = _controller.Error();

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);

        var viewResult = (ViewResult)result;

        Assert.IsInstanceOf<ErrorViewModel>(viewResult.Model);

        var errorViewModel = (ErrorViewModel)viewResult.Model;

        Assert.AreEqual("TestTraceIdentifier", errorViewModel.RequestId);
    }
}
