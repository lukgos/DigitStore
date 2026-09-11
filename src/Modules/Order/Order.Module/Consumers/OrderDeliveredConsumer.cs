using Distribution.Contracts.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Order.Module.Repositories;
using Shared.Abstractions.ValueObjects;

namespace Order.Module.Consumers;

public class OrderDeliveredConsumer(IOrderRepository orderRepository, ILogger<OrderDeliveredConsumer> logger) : IConsumer<OrderDelivered>
{
    public async Task Consume(ConsumeContext<OrderDelivered> context)
    {
        var orderId = new OrderId(context.Message.OrderId);

        var order = await orderRepository.GetByIdAsync(orderId, context.CancellationToken);


        order.MarkAsDelivered();
        
        await orderRepository.SaveChangesAsync(context.CancellationToken);
        
        logger.LogInformation($"Order {orderId} was delivered at {context.Message.DeliveredAt}");

    }
}