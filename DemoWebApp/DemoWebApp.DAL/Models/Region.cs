using System.Diagnostics.CodeAnalysis;

namespace DemoWebApp.DAL.Models;

[ExcludeFromCodeCoverage]
public class Region
{
    public int RegionId { get; set; }

    public string RegionDescription { get; set; }

    public ICollection<Territory> Territories { get; } = new HashSet<Territory>();
}
