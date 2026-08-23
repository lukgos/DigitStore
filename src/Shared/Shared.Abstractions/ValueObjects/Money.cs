using Shared.Abstractions.Exceptions;

namespace Shared.Abstractions.ValueObjects;

public record Money
{
    public decimal Value { get; }

    public Money(decimal value)
    {
        if (value < 0)
        {
            throw new InvalidMoneyException();
        }
        
        Value = value;
    }
    
    public static implicit operator decimal(Money money) => money.Value;
    public static implicit operator Money(decimal value) => new(value);
}