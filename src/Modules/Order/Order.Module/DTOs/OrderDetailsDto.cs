namespace Order.Module.DTOs;

public record OrderDetailsDto(Guid Id, Guid CustomerId, string Status, decimal TotalPrice, IEnumerable<OrderItemDetailsDto> Items);