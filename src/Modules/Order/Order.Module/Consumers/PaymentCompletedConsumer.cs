using MassTransit;
using Order.Contracts.Events;
using Order.Module.Repositories;
using Payment.Contracts.Events;
using Shared.Abstractions.ValueObjects;

namespace Order.Module.Consumers;

public sealed class PaymentCompletedConsumer(IOrderRepository orderRepository) : IConsumer<PaymentCompleted>
{
    public async Task Consume(ConsumeContext<PaymentCompleted> context)
    {
        var orderId = new OrderId(context.Message.OrderId);

        var order = await orderRepository.GetByIdAsync(orderId, context.CancellationToken);


        order.MarkAsPaid();
        
        await orderRepository.SaveChangesAsync(context.CancellationToken);

        await context.Publish(new OrderPaid(order.Id.Value));
    }
}