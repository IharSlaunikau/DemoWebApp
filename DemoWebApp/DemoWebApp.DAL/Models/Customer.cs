using System.Diagnostics.CodeAnalysis;

namespace DemoWebApp.DAL.Models;

[ExcludeFromCodeCoverage]
public class Customer
{
    public string CustomerId { get; set; }

    public string CompanyName { get; set; }

    public string ContactName { get; set; }

    public string ContactTitle { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string Region { get; set; }

    public string PostalCode { get; set; }

    public string Country { get; set; }

    public string Phone { get; set; }

    public string Fax { get; set; }

    public ICollection<Order> Orders { get; } = new HashSet<Order>();

    public ICollection<CustomerDemographic> CustomerTypes { get; } = new HashSet<CustomerDemographic>();
}
