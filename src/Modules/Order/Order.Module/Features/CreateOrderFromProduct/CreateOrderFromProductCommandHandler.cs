using Catalog.Contracts.Services;
using MassTransit;
using Order.Contracts.Events;
using Order.Module.DTOs;
using Order.Module.Exceptions;
using Order.Module.Repositories;
using Order.Module.ValueObjects;
using Shared.Abstractions.CQRS;
using Shared.Abstractions.ValueObjects;

namespace Order.Module.Features.CreateOrderFromProduct;

public sealed class CreateOrderFromProductCommandHandler(IOrderRepository orderRepository, ICatalogApi catalogApi, IPublishEndpoint publishEndpoint) : ICommandHandler<CreateOrderFromProductCommand>
{
    public async Task HandleAsync(CreateOrderFromProductCommand command, CancellationToken ct)
    {
        var product = await catalogApi.GetProductAsync(command.ProductId, ct);
        
        if (product is null)
        {
            throw new ProductNotFoundException(command.ProductId);
        }

        var orderId = new OrderId(command.OrderId);
        var customerId = new UserId(command.CustomerId);
        var productId = new ProductId(command.ProductId);
        var quantity = new Quantity(command.Quantity);
        var unitPrice = new Money(product.Price);
        var totalPrice = quantity * unitPrice;

        var orderItems = new List<OrderItemDto>
        {
            new(productId, quantity, unitPrice)
        };

        var order = Entities.Order.Create(orderId, customerId, orderItems);

        await orderRepository.AddAsync(order, ct);
        await orderRepository.SaveChangesAsync(ct);
        
        await publishEndpoint.Publish(new OrderCreated(orderId, totalPrice), ct);
    }
}