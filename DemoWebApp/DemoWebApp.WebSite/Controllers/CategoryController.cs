using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DemoWebApp.DAL.Interfaces;
using DemoWebApp.DAL.Models;
using DemoWebApp.WebSite.Filters;
using DemoWebApp.WebSite.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApp.WebSite.Controllers;

[ServiceFilter(typeof(LogActionFilter))]
public class CategoryController : Controller
{
    const int BytesToSkip = 78;
    const string ContentType = "image/bmp";

    private const int ImageWidth = 400;
    private const int ImageHeight = 400;
    private const string CropMode = "fill";
    private const string Gravity = "face";
    private const string ImageFolder = "demo_web_app/categories";

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

        var categoryViewModels = categories.Select(category => new CategoryViewModel
        {
            Id = category.Id,
            CategoryName = category.CategoryName,
            PictureUrl = category.PictureUrl,
            Picture = category.Picture
        });

        return View(ViewNames.Index, categoryViewModels);
    }

    [HttpGet("{id}/image")]
    public async Task<IActionResult> GetImage(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        if (category.Picture != null)
        {
            var fixedImageData = category.Picture.Skip(BytesToSkip).ToArray();
            return File(fixedImageData, ContentType);
        }

        if (category.PictureUrl != null)
        {
            return Redirect(category.PictureUrl.AbsoluteUri);
        }

        return NotFound();
    }

    [HttpGet("{id}/EditImage")]
    public async Task<IActionResult> EditImageGet([FromRoute] int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        var editCategoryImageViewModel = category.Adapt<CategoryViewModel>();

        editCategoryImageViewModel.PictureUrl = category.Picture != null
            ? new Uri($"{Request.Scheme}://{Request.Host}{Url.Action(nameof(GetImage), new { id = category.Id })}")
            : category.PictureUrl;

        return View(ViewNames.EditImage, editCategoryImageViewModel);
    }

    [HttpPost("{id}/EditImage")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditImagePost([FromRoute] int id, [FromForm] CategoryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(ViewNames.EditImage, model);
        }

        if (model.NewImage != null)
        {
            await using var imageStream = model.NewImage.OpenReadStream();

            var uploadResult = await _cloudinary.UploadAsync(new ImageUploadParams
            {
                File = new FileDescription(model.NewImage.FileName, imageStream),
                Transformation = new Transformation().Width(ImageWidth).Height(ImageHeight).Crop(CropMode).Gravity(Gravity),
                Folder = ImageFolder
            });

            if (uploadResult.Error != null)
            {
                ModelState.AddModelError(string.Empty, $"Failed to upload image: {uploadResult.Error.Message}");

                return View(ViewNames.EditImage, model);
            }

            model.PictureUrl = uploadResult.Url;
        }

        var categoryToUpdate = await _categoryRepository.GetByIdAsync(id);

        if (categoryToUpdate == null)
        {
            return NotFound();
        }

        var previousCategory = categoryToUpdate.Clone();

        model.Adapt(categoryToUpdate);

        if (model.NewImage != null)
        {
            categoryToUpdate.Picture = null;
        }

        await _categoryRepository.UpdateFieldsAsync(categoryToUpdate, (Category)previousCategory);

        return RedirectToAction(ViewNames.Index, categoryToUpdate);
    }
}
