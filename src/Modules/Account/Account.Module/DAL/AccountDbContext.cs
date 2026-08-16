using System.Reflection;
using Account.Module.Entities;
using Microsoft.EntityFrameworkCore;

namespace Account.Module.DAL;

public sealed class AccountDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("account");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}