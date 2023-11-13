using System.Diagnostics.CodeAnalysis;

namespace DemoWebApp.DAL.Models;

[ExcludeFromCodeCoverage]
public class Shipper
{
    public int ShipperId { get; set; }

    public string CompanyName { get; set; }

    public string Phone { get; set; }

    public ICollection<Order> Orders { get; } = new HashSet<Order>();
}
