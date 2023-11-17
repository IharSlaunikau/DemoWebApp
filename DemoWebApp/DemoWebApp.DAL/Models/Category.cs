using System.ComponentModel.DataAnnotations.Schema;

namespace DemoWebApp.DAL.Models;

public class Category : BaseEntity, ICloneable
{
    [Column("CategoryId")]
    public override int Id { get; set; }

    public string CategoryName { get; set; }

    public string Description { get; set; }

    public byte[] Picture { get; set; }

    public Uri PictureUrl { get; set; }

    public ICollection<Product> Products { get; } = new HashSet<Product>();

    public object Clone()
    {
        return MemberwiseClone();
    }
}
