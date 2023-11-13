using System.Diagnostics.CodeAnalysis;

namespace DemoWebApp.DAL.Models;

[ExcludeFromCodeCoverage]
public partial class ProductsAboveAveragePrice
{
    public string ProductName { get; set; }

    public decimal? UnitPrice { get; set; }
}
