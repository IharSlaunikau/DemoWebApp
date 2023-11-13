using DemoWebApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoWebApp.DAL.ConfigurationEntities;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(product => product.Id);

        builder.HasMany(product => product.Products)
            .WithOne(product => product.Category)
            .HasForeignKey(product => product.CategoryId);
    }
}
