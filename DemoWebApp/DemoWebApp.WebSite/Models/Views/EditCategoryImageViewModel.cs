using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DemoWebApp.WebSite.Models.Views;

public class EditCategoryImageViewModel
{
    [HiddenInput]
    public int Id { get; set; }

    [Display(Name = "Category Name")]
    [Required(ErrorMessage = "Please enter the category name")]
    [MaxLength(15, ErrorMessage = "The category name must be 15 characters or less")]
    public string CategoryName { get; set; }

    [Display(Name = "Picture")]
    public string PictureUrl { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "New Image")]
    public IFormFile NewImage { get; set; }
}
