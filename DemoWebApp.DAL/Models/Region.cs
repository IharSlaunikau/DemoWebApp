namespace DemoWebApp.DAL.Models;

public class Region
{
    public int RegionId { get; set; }

    public string RegionDescription { get; set; }

    public ICollection<Territory> Territories { get; } = new HashSet<Territory>();
}
