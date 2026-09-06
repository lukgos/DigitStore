using Order.Module.Repositories;

namespace Order.Module.DAL.Repositories;

public sealed class OrderRepository(OrderDbContext dbContext) : IOrderRepository
{
    public async Task AddAsync(Entities.Order order, CancellationToken ct)
    {
        await dbContext.Orders.AddAsync(order, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await dbContext.SaveChangesAsync(ct);
    }
}