using Catalog.Module.Entities;
using Catalog.Module.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Abstractions.ValueObjects;

namespace Catalog.Module.DAL.Configurations;

public sealed class ProductPriceHistoryConfiguration : IEntityTypeConfiguration<ProductPriceHistory>
{
    public void Configure(EntityTypeBuilder<ProductPriceHistory> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new ProductPriceHistoryId(x));

        builder.Property(x => x.ProductId)
            .HasConversion(x => x.Value, x => new ProductId(x));

        builder.Property(x => x.Price)
            .HasConversion(x => x.Value, x => new Money(x))
            .HasColumnType("numeric(18,2)");
            
        builder.Property(x => x.ValidFrom)
            .IsRequired();
    }
}