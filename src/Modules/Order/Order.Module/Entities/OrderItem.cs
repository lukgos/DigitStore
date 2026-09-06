using Order.Module.ValueObjects;
using Shared.Abstractions.Common;
using Shared.Abstractions.ValueObjects;

namespace Order.Module.Entities;

public class OrderItem : Entity<OrderItemId>
{
    public OrderId OrderId { get; private set; }
    public ProductId ProductId { get; private set; }
    public Quantity Quantity { get; private set; }
    public Money UnitPrice { get; private set; }
    public Money TotalPrice => new(UnitPrice.Value * Quantity.Value);

    private OrderItem()
    {
        
    }

    public OrderItem(OrderId orderId, ProductId productId, Quantity quantity, Money unitPrice)
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public static OrderItem Create(OrderId orderId, ProductId productId, Quantity quantity, Money unitPrice)
    {
        return new OrderItem(orderId, productId, quantity, unitPrice)
        {
            Id = new OrderItemId(Guid.NewGuid())
        };
    }
}