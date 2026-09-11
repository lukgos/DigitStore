namespace Distribution.Contracts.Events;

public record OrderDelivered(Guid OrderId, DateTime DeliveredAt);