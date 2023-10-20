using DemoWebApp.DAL.Models;
using DemoWebApp.WebSite.Models;
using Mapster;

namespace DemoWebApp.WebSite;

public static class MappingConfig
{
    private const bool ShouldPreserveReference = true;

    public static void Configure()
    {
        TypeAdapterConfig<Product, ProductViewModel>.NewConfig()
            .Map(dest => dest.CategoryName, src => src.Category.CategoryName)
            .TwoWays();

        TypeAdapterConfig<Category, CategoryViewModel>.NewConfig()
            .PreserveReference(ShouldPreserveReference)
            .TwoWays();
    }
}
