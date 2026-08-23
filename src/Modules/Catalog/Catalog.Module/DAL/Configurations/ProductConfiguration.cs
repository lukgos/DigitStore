using Catalog.Module.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Abstractions.ValueObjects;

namespace Catalog.Module.DAL.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new ProductId(x));
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.Description)
            .HasMaxLength(1000);
        
        builder.Property(x => x.Price)
            .HasConversion(x => x.Value, x => new Money(x))
            .HasColumnType("numeric(18,2)");
            
        builder.Property(x => x.CategoryId)
            .HasConversion(x => x.Value, x => new CategoryId(x));
        
        builder.Property(x => x.Version).IsConcurrencyToken();

        builder.Property(x => x.Tags)
            .HasField("_tags")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(x => x.Attributes)
            .HasField("_attributes")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnType("jsonb");

        builder.HasOne(x => x.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(x => x.Images)
            .WithOne(i => i.Product)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.PriceHistory)
            .WithOne(h => h.Product)
            .HasForeignKey(h => h.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}