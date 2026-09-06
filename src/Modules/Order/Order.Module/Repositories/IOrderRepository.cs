namespace Order.Module.Repositories;

public interface IOrderRepository
{
    Task AddAsync(Entities.Order order, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}