using System.Diagnostics.CodeAnalysis;

namespace DemoWebApp.DAL.Models;

[ExcludeFromCodeCoverage]
public partial class ProductSalesFor1997
{
    public string CategoryName { get; set; }

    public string ProductName { get; set; }

    public decimal? ProductSales { get; set; }
}
