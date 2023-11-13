using System.Diagnostics.CodeAnalysis;

namespace DemoWebApp.DAL.Models;

[ExcludeFromCodeCoverage]
public class SummaryOfSalesByYear
{
    public DateTime? ShippedDate { get; set; }

    public int OrderId { get; set; }

    public decimal? Subtotal { get; set; }
}
