using Order.Module.DTOs;
using Order.Module.Exceptions;
using Shared.Abstractions.Common;
using Shared.Abstractions.ValueObjects;

namespace Order.Module.Entities;

public class Order : AuditableEntity<OrderId>
{
    private readonly List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    
    public UserId CustomerId { get; private set; } 
    public Money TotalPrice => new(_items.Sum(i => i.TotalPrice.Value));
    public OrderStatus Status { get; private set; }

    private Order()
    {
        
    }

    private Order(OrderId id, UserId customerId, OrderStatus status)
    {
        Id = id;
        CustomerId = customerId;
        Status = status;
    }

    public static Order Create(OrderId id, UserId customerId, IEnumerable<OrderItemDto> items)  
    {
        var order = new Order(id, customerId, OrderStatus.Pending)
        {
            Id = id
        };

        foreach (var item in items)
        {
            order._items.Add(OrderItem.Create(id, item.ProductId, item.Quantity, item.UnitPrice));
        }

        if (order._items.Count == 0)
        {
            throw new EmptyOrderException();
        }

        return order;
    }

    public void MarkAsPaid()
    {
        if (Status != OrderStatus.Pending)
        {
            throw new InvalidOrderStateException();
        }
        
        Status = OrderStatus.Paid;
    }
    
    public void MarkAsDelivered()
    {
        if (Status != OrderStatus.Paid)
        {
            throw new InvalidOrderStateException(); 
        }
    
        Status = OrderStatus.Delivered;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Paid)
        {
            throw new InvalidOrderStateException();
        }

        Status = OrderStatus.Canceled;
    }
}