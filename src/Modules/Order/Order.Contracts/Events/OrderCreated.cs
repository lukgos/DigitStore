namespace Order.Contracts.Events;

public record OrderCreated(Guid OrderId, decimal TotalAmount);