using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
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
    private readonly Cloudinary _cloudinary;

    public CategoryController(ICategoryRepository categoryRepository, Cloudinary cloudinary)
    {
        ArgumentNullException.ThrowIfNull(categoryRepository);
        ArgumentNullException.ThrowIfNull(cloudinary);

        _categoryRepository = categoryRepository;
        _cloudinary = cloudinary;
    }

    [HttpGet]
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

    [HttpGet("{id}/editimage")]
    public async Task<IActionResult> EditImageGet([FromRoute] int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        var editCategoryImageViewModel = new EditCategoryImageViewModel()
        {
            Id = category.Id,
            CategoryName = category.CategoryName,
            PictureUrl = category.PictureUrl
        };

        return View(ViewNames.EditImage, editCategoryImageViewModel);
    }

    [HttpPost("{id}/editimage")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditImagePost([FromRoute] int id, EditCategoryImageViewModel editCategoryImageViewModel)
    {
        ArgumentNullException.ThrowIfNull(editCategoryImageViewModel);

        if (editCategoryImageViewModel.NewImage == null || editCategoryImageViewModel.NewImage.Length == 0)
        {
            ModelState.AddModelError("NewImage", "Please select an image to upload.");

            var category = await _categoryRepository.GetByIdAsync(id);

            editCategoryImageViewModel.CategoryName = category.CategoryName;
            editCategoryImageViewModel.PictureUrl = category.PictureUrl;

            return View(ViewNames.EditImage, editCategoryImageViewModel);
        }

        var categoryToUpdate = await _categoryRepository.GetByIdAsync(id);

        if (categoryToUpdate == null)
        {
            return NotFound();
        }

        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(editCategoryImageViewModel.NewImage.FileName,
                editCategoryImageViewModel.NewImage.OpenReadStream())
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        categoryToUpdate.PictureUrl = result.SecureUrl.AbsoluteUri;

        await _categoryRepository.UpdateFieldsAsync(categoryToUpdate, new Category());

        return RedirectToAction(ViewNames.Index);
    }
}
