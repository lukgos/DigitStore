using Catalog.Module.Entities;
using Catalog.Module.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Abstractions.ValueObjects;

namespace Catalog.Module.DAL.Configurations;

public sealed class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new ProductImageId(x));

        builder.Property(x => x.ProductId)
            .HasConversion(x => x.Value, x => new ProductId(x));
        
        builder.Property(x => x.Url)
            .IsRequired()
            .HasMaxLength(2000);
        
        builder.Property(x => x.AltText)
            .HasMaxLength(500);
    }
}