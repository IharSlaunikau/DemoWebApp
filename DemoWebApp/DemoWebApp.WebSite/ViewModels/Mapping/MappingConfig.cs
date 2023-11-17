using System.Diagnostics.CodeAnalysis;
using DemoWebApp.DAL.Models;
using Mapster;

namespace DemoWebApp.WebSite.ViewModels.Mapping;

[ExcludeFromCodeCoverage]
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
