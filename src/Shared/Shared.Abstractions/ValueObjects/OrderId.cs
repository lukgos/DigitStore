using Shared.Abstractions.Exceptions;

namespace Shared.Abstractions.ValueObjects;

public record OrderId
{
    public Guid Value { get; }

    public OrderId(Guid orderId)
    {
        if (orderId == Guid.Empty)
        {
            throw new InvalidOrderIdException();
        }
        
        Value = orderId;
    }
    
    public static implicit operator Guid(OrderId orderId) => orderId.Value;
    public static implicit operator OrderId(Guid orderId) => new(orderId);
}