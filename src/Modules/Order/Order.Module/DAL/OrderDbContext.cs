using Microsoft.EntityFrameworkCore;

namespace Order.Module.DAL;

public class OrderDbContext : DbContext
{
    public DbSet<Entities.Order> Orders { get; set; }
    public DbSet<Entities.OrderItem> OrderItems { get; set; }
    
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("order");
        
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
    
}