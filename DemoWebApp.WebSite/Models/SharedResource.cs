namespace DemoWebApp.WebSite.Models;

public static class SharedResource
{
    public static string ProductNameRequired => "Product name is required.";

    public static string ProductNameMaxLength => "Product name should be less than 50 characters.";

    public static string UnitPriceRequired => "Unit price is required.";

    public static string UnitPriceRange => "Unit price must be a positive number.";

    public static string UnitsInStockRequired => "Units in stock is required.";

    public static string UnitsInStockRange => "Units in stock must be a non-negative integer.";

    public static string GetResource(string resourceName)
    {
        return typeof(SharedResource).GetProperty(resourceName)?.GetValue(null, null)?.ToString();
    }
}
