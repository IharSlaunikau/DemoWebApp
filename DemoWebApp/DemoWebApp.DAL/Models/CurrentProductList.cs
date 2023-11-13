using System.Diagnostics.CodeAnalysis;

namespace DemoWebApp.DAL.Models;

[ExcludeFromCodeCoverage]
public class CurrentProductList
{
    public int ProductId { get; set; }

    public string ProductName { get; set; }
}
