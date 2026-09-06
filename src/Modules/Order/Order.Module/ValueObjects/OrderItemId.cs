using Order.Module.Exceptions;

namespace Order.Module.ValueObjects;

public record OrderItemId
{
    public Guid Value { get; }

    public OrderItemId(Guid orderItemId)
    {
        if (orderItemId == Guid.Empty)
        {
            throw new InvalidOrderItemIdException();
        }
        
        Value = orderItemId;
    }
    
    public static implicit operator Guid(OrderItemId orderItemId) => orderItemId.Value;
    public static implicit operator OrderItemId(Guid orderItemId) => new(orderItemId);
}