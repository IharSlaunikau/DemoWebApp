using System.Diagnostics.CodeAnalysis;

namespace DemoWebApp.WebSite.Models.Views;

[ExcludeFromCodeCoverage]
public class CategoryViewModel
{
    public int Id { get; set; }

    public string CategoryName { get; set; }

    public string Description { get; set; }

#pragma warning disable CA1819 // Properties should not return arrays
    public byte[] Picture { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

    public ICollection<ProductViewModel> Products { get; } = new HashSet<ProductViewModel>();
}
