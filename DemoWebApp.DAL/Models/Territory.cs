namespace DemoWebApp.DAL.Models;

public class Territory
{
    public string TerritoryId { get; set; }

    public string TerritoryDescription { get; set; }

    public int RegionId { get; set; }

    public Region Region { get; set; }

    public ICollection<Employee> Employees { get; } = new HashSet<Employee>();
}
