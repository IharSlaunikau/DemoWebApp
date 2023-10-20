using DemoWebApp.DAL.Interfaces;
using DemoWebApp.WebSite.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApp.WebSite.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository)
    {
        ArgumentNullException.ThrowIfNull(categoryRepository);

        _categoryRepository = categoryRepository;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _categoryRepository.GetAllAsync();

        var categoryViewModels = categories.Adapt<IEnumerable<CategoryViewModel>>();

        return View(categoryViewModels);
    }
}