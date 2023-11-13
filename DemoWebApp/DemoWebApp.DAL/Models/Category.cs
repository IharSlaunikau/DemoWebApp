using System.ComponentModel.DataAnnotations.Schema;

namespace DemoWebApp.DAL.Models;

public class Category : BaseEntity, ICloneable
{
    [Column("CategoryId")]
    public override int Id { get; set; }

    public string CategoryName { get; set; }

    public string Description { get; set; }

#pragma warning disable CA1819 // Properties should not return arrays
    public byte[] Picture { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

    public string PictureUrl { get; set; }

    public ICollection<Product> Products { get; } = new HashSet<Product>();

    public object Clone()
    {
        return MemberwiseClone();
    }
}
