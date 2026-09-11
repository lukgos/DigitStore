using Shared.Abstractions.ValueObjects;

namespace Order.Module.Repositories;

public interface IOrderRepository
{
    Task AddAsync(Entities.Order order, CancellationToken ct);
    Task<Entities.Order?> GetByIdAsync(OrderId id, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}