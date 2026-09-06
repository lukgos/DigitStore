using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Abstractions.ValueObjects;
using Shared.EntityFramework;

namespace Order.Module.DAL.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Entities.Order>
{
    public void Configure(EntityTypeBuilder<Entities.Order> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Status)
            .HasEnumConversion();
        
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new OrderId(x));

        builder.Property(x => x.CustomerId)
            .HasConversion(x => x.Value, x => new UserId(x));

        builder.Ignore(x => x.TotalPrice);
        

        builder.Property(x => x.Version).IsConcurrencyToken();

        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.Navigation(x => x.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}