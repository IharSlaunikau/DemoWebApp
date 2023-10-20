using System.ComponentModel.DataAnnotations;

namespace DemoWebApp.WebSite.Models;

public class ProductViewModel
{
    public int ProductId { get; set; }

    [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = nameof(SharedResource.ProductNameRequired))]
    [StringLength(50, ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = nameof(SharedResource.ProductNameMaxLength))]
    public string ProductName { get; set; }

    public int? SupplierId { get; set; }

    public int? CategoryId { get; set; }

    public string QuantityPerUnit { get; set; }

    [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = nameof(SharedResource.UnitPriceRequired))]
    [Range(0, double.MaxValue, ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = nameof(SharedResource.UnitPriceRange))]
    public decimal? UnitPrice { get; set; }

    [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = nameof(SharedResource.UnitsInStockRequired))]
    [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = nameof(SharedResource.UnitsInStockRange))]
    public short? UnitsInStock { get; set; }

    public short? UnitsOnOrder { get; set; }

    public short? ReorderLevel { get; set; }

    public bool Discontinued { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public string CategoryName { get; set; }
}
