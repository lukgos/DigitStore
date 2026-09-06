using Order.Module.DTOs;
using Shared.Abstractions.CQRS;

namespace Order.Module.Features.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId) : IQuery<OrderDetailsDto?>;