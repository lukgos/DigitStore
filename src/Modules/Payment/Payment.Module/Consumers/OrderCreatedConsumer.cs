using MassTransit;
using Order.Contracts.Events;
using Payment.Contracts.Events;

namespace Payment.Module.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreated>
{
    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        var orderId = context.Message.OrderId;

        await Task.Delay(10000, context.CancellationToken);
        
        await context.Publish(new PaymentCompleted(orderId));
    }
}