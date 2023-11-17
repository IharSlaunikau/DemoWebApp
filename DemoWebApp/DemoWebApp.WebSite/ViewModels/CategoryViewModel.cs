using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApp.WebSite.ViewModels;

[ExcludeFromCodeCoverage]
public class CategoryViewModel
{
    [HiddenInput]
    public int Id { get; set; }

    [Display(Name = "Category Name")]
    [Required(ErrorMessage = "Please enter the category name")]
    [MaxLength(15, ErrorMessage = "The category name must be 15 characters or less")]
    public string CategoryName { get; set; }

    public string Description { get; set; }

    public byte[] Picture { get; set; }

    [Display(Name = "Picture")]
    public Uri PictureUrl { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "New Image")]
    public IFormFile NewImage { get; set; }

    public ICollection<ProductViewModel> Products { get; } = new HashSet<ProductViewModel>();
}
