namespace DemoWebApp.DAL.Models;

public class CustomerDemographic
{

    public string CustomerTypeId { get; set; }

    public string CustomerDesc { get; set; }

    public ICollection<Customer> Customers { get; } = new HashSet<Customer>();
}
