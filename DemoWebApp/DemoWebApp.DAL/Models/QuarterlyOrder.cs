using System.Diagnostics.CodeAnalysis;

namespace DemoWebApp.DAL.Models;

[ExcludeFromCodeCoverage]
public class QuarterlyOrder
{
    public string CustomerId { get; set; }

    public string CompanyName { get; set; }

    public string City { get; set; }

    public string Country { get; set; }
}
