using Microsoft.EntityFrameworkCore;
using Order.Module.Repositories;
using Shared.Abstractions.ValueObjects;

namespace Order.Module.DAL.Repositories;

public sealed class OrderRepository(OrderDbContext dbContext) : IOrderRepository
{
    public async Task AddAsync(Entities.Order order, CancellationToken ct)
    {
        await dbContext.Orders.AddAsync(order, ct);
    }
    
    public async Task<Entities.Order?> GetByIdAsync(OrderId id, CancellationToken ct)
    {
        return await dbContext.Orders.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await dbContext.SaveChangesAsync(ct);
    }
}