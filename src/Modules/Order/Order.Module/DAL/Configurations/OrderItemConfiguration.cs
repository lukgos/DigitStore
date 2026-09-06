using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Module.Entities;
using Order.Module.ValueObjects;
using Shared.Abstractions.ValueObjects;

namespace Order.Module.DAL.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new OrderItemId(x));

        builder.Property(x => x.OrderId)
            .HasConversion(x => x.Value, x => new OrderId(x));

        builder.Property(x => x.ProductId)
            .HasConversion(x => x.Value, x => new ProductId(x));

        builder.Property(x => x.Quantity)
            .HasConversion(x => x.Value, x => new Quantity(x));

        builder.Property(x => x.UnitPrice)
            .HasConversion(x => x.Value, x => new Money(x))
            .HasColumnType("numeric(18,2)");
            
        builder.Ignore(x => x.TotalPrice);
    }
}