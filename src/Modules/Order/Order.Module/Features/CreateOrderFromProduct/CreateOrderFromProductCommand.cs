using Shared.Abstractions.CQRS;

namespace Order.Module.Features.CreateOrderFromProduct;

public record CreateOrderFromProductCommand(Guid OrderId, Guid CustomerId, Guid ProductId, int Quantity) : ICommand;