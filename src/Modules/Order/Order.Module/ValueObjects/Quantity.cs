using Order.Module.Exceptions;

namespace Order.Module.ValueObjects;

public record Quantity
{
    public int Value { get; }

    public Quantity(int value)
    {
        if (value <= 0)
        {
            throw new InvalidQuantityException();
        }
        
        Value = value;
    }

    public static implicit operator int(Quantity quantity) => quantity.Value;
    public static implicit operator Quantity(int value) => new(value);
}