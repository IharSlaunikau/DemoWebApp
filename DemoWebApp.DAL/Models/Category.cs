namespace DemoWebApp.DAL.Models;

public class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public string Description { get; set; }

#pragma warning disable CA1819 // Properties should not return arrays
    public byte[] Picture { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

    public ICollection<Product> Products { get; } = new HashSet<Product>();
}
