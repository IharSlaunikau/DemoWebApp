using System.Globalization;
using DemoWebApp.DAL.Interfaces;
using DemoWebApp.DAL.Models;
using DemoWebApp.WebSite.Filters;
using DemoWebApp.WebSite.Settings;
using DemoWebApp.WebSite.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace DemoWebApp.WebSite.Controllers;

[ServiceFilter(typeof(LogActionFilter))]
public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ProductSettings _productSettings;

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

        _productSettings = productSettings.Value;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var amount = _productSettings.MaxAmount;

        var products = await _productRepository.GetLimitedProducts(amount);

        ViewBag.MaxAmount = amount;

        var productViewModels = products.Adapt<IEnumerable<ProductViewModel>>();

        return View(ViewNames.Index, productViewModels);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await GetCategoriesViewModel();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductViewModel productViewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await GetCategoriesViewModel();

            return View(ViewNames.Create, productViewModel);
        }

        var product = productViewModel.Adapt<Product>();

        await _productRepository.AddAsync(product);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{id}/edit")]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        var productViewModel = product.Adapt<ProductViewModel>();

        ViewBag.Categories = await GetCategoriesViewModel();

        return View(ViewNames.Edit, productViewModel);
    }

    [HttpPost("{id}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductViewModel productViewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await GetCategoriesViewModel();

            return View(ViewNames.Edit, productViewModel);
        }

        var productToUpdate = await _productRepository.GetByIdAsync(id);

        if (productToUpdate == null)
        {
            return NotFound();
        }

        var previousProduct = productToUpdate.Clone();

        productToUpdate = productViewModel.Adapt(productToUpdate);

        await _productRepository.UpdateFieldsAsync(productToUpdate, (Product)previousProduct);

        return RedirectToAction(nameof(Index));
    }

    private async Task<IEnumerable<SelectListItem>> GetCategoriesViewModel()
    {
        var categories = await _categoryRepository.GetAllAsync();

        var categoryViewModels = categories.Adapt<IEnumerable<CategoryViewModel>>();

        return categoryViewModels
            .Select(viewModel => new SelectListItem
            {
                Text = viewModel.CategoryName,
                Value = viewModel.Id.ToString(CultureInfo.InvariantCulture)
            });
    }
}
