using Order.Module.ValueObjects;
using Shared.Abstractions.ValueObjects;

namespace Order.Module.DTOs;

public record OrderItemDto(ProductId ProductId, Quantity Quantity, Money UnitPrice);