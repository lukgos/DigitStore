using Account.Module.Entities;
using Account.Module.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Abstractions.ValueObjects;

namespace Account.Module.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
            
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new UserId(x));

        builder.Property(x => x.Email)
            .HasConversion(x => x.Value, x => new Email(x))
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.PasswordHash)
            .HasConversion(x => x.Value, x => new PasswordHash(x))
            .IsRequired();
        
        builder.Property(x => x.Role)
            .HasConversion(x => x.Value, x => new Role(x))
            .IsRequired()
            .HasMaxLength(30);

        builder.HasIndex(x => x.Email).IsUnique();
    }
}