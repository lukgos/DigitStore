using Catalog.Module.Exceptions;

namespace Catalog.Module.ValueObjects;

public record ProductPriceHistoryId
{
    public Guid Value { get; }

    public ProductPriceHistoryId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidProductPriceHistoryIdException();
        }
        
        Value = value;
    }
    
    public static implicit operator Guid(ProductPriceHistoryId id) => id.Value;
    public static implicit operator ProductPriceHistoryId(Guid id) => new(id);
}