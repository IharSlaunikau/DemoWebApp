namespace DemoWebApp.WebSite.Models;

public class CategoryViewModel
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public string Description { get; set; }

    public ICollection<byte> Picture { get; }

    public ICollection<ProductViewModel> Products { get; } = new HashSet<ProductViewModel>();
}
