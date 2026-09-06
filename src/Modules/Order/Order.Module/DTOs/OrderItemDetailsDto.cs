namespace Order.Module.DTOs;

public record OrderItemDetailsDto(Guid ProductId, int Quantity, decimal UnitPrice, decimal TotalPrice);