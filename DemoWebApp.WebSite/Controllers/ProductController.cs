using System.Globalization;
using DemoWebApp.DAL.Interfaces;
using DemoWebApp.DAL.Models;
using DemoWebApp.WebSite.Models;
using DemoWebApp.WebSite.Settings;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace DemoWebApp.WebSite.Controllers;

public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    private readonly int _maxAmount;

    public ProductController
        (
            IProductRepository productRepository,
            IOptions<ProductSettings> productSettings,
            ICategoryRepository categoryRepository
        )
    {
        ArgumentNullException.ThrowIfNull(productRepository);
        ArgumentNullException.ThrowIfNull(categoryRepository);
        ArgumentNullException.ThrowIfNull(productSettings);

        _productRepository = productRepository;
        _categoryRepository = categoryRepository;

        _maxAmount = productSettings.Value.MaxAmount;
    }

    public async Task<IActionResult> Index(int maxAmount)
    {
        var products = await _productRepository.GetLimitedProducts(_maxAmount);

        ViewBag.MaxAmount = maxAmount;

        var productViewModels = products.Adapt<IEnumerable<ProductViewModel>>();

        return View(productViewModels);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await GetCategoriesViewModel();

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductViewModel productViewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await GetCategoriesViewModel();

            return View(productViewModel);
        }

        var product = productViewModel.Adapt<Product>();

        await _productRepository.AddAsync(product);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        var productViewModel = product.Adapt<ProductViewModel>();

        ViewBag.Categories = await GetCategoriesViewModel();

        return View(productViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, ProductViewModel productViewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await GetCategoriesViewModel();

            return View(productViewModel);
        }

        var productToUpdate = await _productRepository.GetByIdAsync(id);

        if (productToUpdate == null)
        {
            return NotFound();
        }

        productToUpdate = productViewModel.Adapt(productToUpdate);

        await _productRepository.UpdateFieldsAsync(productToUpdate, productToUpdate);

        return RedirectToAction(nameof(Index));
    }

    private async Task<IEnumerable<SelectListItem>> GetCategoriesViewModel()
    {
        var categories = await _categoryRepository.GetAllAsync();

        var categoryViewModels = categories.Adapt<IEnumerable<CategoryViewModel>>();

        return categoryViewModels
            .Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = c.CategoryId.ToString(CultureInfo.InvariantCulture)
            });
    }
}
