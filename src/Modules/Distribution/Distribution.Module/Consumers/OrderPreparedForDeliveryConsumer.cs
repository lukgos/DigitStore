using Distribution.Contracts.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Order.Contracts.Events;

namespace Distribution.Module.Consumers;

public class OrderPreparedForDeliveryConsumer(ILogger<OrderPreparedForDeliveryConsumer> logger) : IConsumer<OrderPaid>
{
    public async Task Consume(ConsumeContext<OrderPaid> context)
    {
        var orderId = context.Message.OrderId;
        
        
        logger.LogInformation("Order {OrderId} delivered", orderId);
        
        await Task.Delay(5000, context.CancellationToken);

        await context.Publish(new OrderDelivered(orderId, DateTime.UtcNow));
    }
}