using System.Diagnostics.CodeAnalysis;

namespace DemoWebApp.DAL.Models;

[ExcludeFromCodeCoverage]
public class OrderSubtotal
{
    public int OrderId { get; set; }

    public decimal? Subtotal { get; set; }
}
