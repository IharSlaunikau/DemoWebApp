using DemoWebApp.DAL.Interfaces;
using DemoWebApp.DAL.Models;
using DemoWebApp.WebSite.Models.Views;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApp.WebSite.Controllers;

public class CategoryController : Controller
{
    const int BytesToSkip = 78;
    const string ContentType = "image/bmp";

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

        return View(ViewNames.Index, categoryViewModels);
    }

    [HttpGet("{id}/image")]
    public async Task<IActionResult> GetImage(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category?.Picture == null)
        {
            return NotFound();
        }

        var fixedImageData = category.Picture.Skip(BytesToSkip).ToArray();

        return File(fixedImageData, ContentType);
    }

    public async Task<IActionResult> EditImage(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        var model = new EditCategoryImageViewModel
        {
            Id = category.Id,
            CategoryName = category.CategoryName,
            Picture = category.Picture
        };

        return View(model);
    }

    [HttpPost("{id}/editimage")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditImagePost([FromRoute] int id, EditCategoryImageViewModel editCategoryImageViewModel)
    {
        if (editCategoryImageViewModel.NewImage == null || editCategoryImageViewModel.NewImage.Length == 0)
        {
            ModelState.AddModelError("NewImage", "Please select an image to upload.");

            var category = await _categoryRepository.GetByIdAsync(id);

            editCategoryImageViewModel.CategoryName = category.CategoryName;
            editCategoryImageViewModel.Picture = category.Picture;

            return View("EditImage", editCategoryImageViewModel);
        }

        var categoryToUpdate = await _categoryRepository.GetByIdAsync(id);

        if (categoryToUpdate == null)
        {
            return NotFound();
        }

        using (var memoryStream = new MemoryStream())
        {
            await editCategoryImageViewModel.NewImage.CopyToAsync(memoryStream);

            var previousCategory = categoryToUpdate.Clone();
            categoryToUpdate.Picture = memoryStream.ToArray();

            await _categoryRepository.UpdateFieldsAsync(categoryToUpdate, (Category)previousCategory);
        }

        return RedirectToAction("Index");
    }
}
