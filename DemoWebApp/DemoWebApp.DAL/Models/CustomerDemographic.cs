using System.Diagnostics.CodeAnalysis;

namespace DemoWebApp.DAL.Models;

[ExcludeFromCodeCoverage]
public class CustomerDemographic
{

    public string CustomerTypeId { get; set; }

    public string CustomerDesc { get; set; }

    public ICollection<Customer> Customers { get; } = new HashSet<Customer>();
}
