namespace DemoWebApp.DAL.Models;

public class Category
{
    private byte[] _picture;

    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public string Description { get; set; }

    public IReadOnlyList<byte> Picture
    {
        get => _picture;
        set => _picture = value?.ToArray();
    }

    public ICollection<Product> Products { get; } = new HashSet<Product>();
}
