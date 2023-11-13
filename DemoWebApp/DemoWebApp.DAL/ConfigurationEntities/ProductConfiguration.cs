using DemoWebApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoWebApp.DAL.ConfigurationEntities;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(product => product.Id);

        builder.HasOne(product => product.Category)
            .WithMany(product => product.Products)
            .HasForeignKey(product => product.CategoryId);
    }
}
