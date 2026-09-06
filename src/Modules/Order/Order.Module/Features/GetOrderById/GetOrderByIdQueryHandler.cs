using Microsoft.EntityFrameworkCore;
using Order.Module.DAL;
using Order.Module.DTOs;
using Shared.Abstractions.Auth;
using Shared.Abstractions.CQRS;
using Shared.Abstractions.ValueObjects;

namespace Order.Module.Features.GetOrderById;

public sealed class GetOrderByIdQueryHandler(OrderDbContext dbContext, IUserContext userContext) : IQueryHandler<GetOrderByIdQuery, OrderDetailsDto?>
{
    public async Task<OrderDetailsDto?> HandleAsync(GetOrderByIdQuery query, CancellationToken ct)
    {
        var order = await dbContext.Orders
            .AsNoTracking()
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == new OrderId(query.OrderId), ct);

        if (order is null)
        {
            return null;
        }

        var isOwner = order.CustomerId.Value == userContext.UserId;
        var isAdmin = userContext.UserRole == "admin";

        if (!isOwner && !isAdmin)
        {
            throw new UnauthorizedAccessException("You have no access to this order.");
        }

        var items = order.Items.Select(i => new OrderItemDetailsDto(i.ProductId.Value, i.Quantity.Value, i.UnitPrice.Value, i.TotalPrice.Value));

        return new OrderDetailsDto(order.Id.Value, order.CustomerId.Value, order.Status.ToString(), order.TotalPrice.Value, items);
    }
}