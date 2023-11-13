using System.Diagnostics.CodeAnalysis;

namespace DemoWebApp.DAL.Models;

[ExcludeFromCodeCoverage]
public class SalesByCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public string ProductName { get; set; }

    public decimal? ProductSales { get; set; }
}
