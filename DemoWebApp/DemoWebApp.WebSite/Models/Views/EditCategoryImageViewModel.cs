namespace DemoWebApp.WebSite.Models.Views;

public class EditCategoryImageViewModel
{
    public int Id { get; set; }

    public string CategoryName { get; set; }

    public byte[] Picture { get; set; }

    public IFormFile NewImage { get; set; }
}
