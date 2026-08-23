using Catalog.Module.Exceptions;

namespace Catalog.Module.ValueObjects;

public record ProductImageId
{
    public Guid Value { get; }

    public ProductImageId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidProductImageIdException();
        }
        
        Value = value;
    }
    
    public static implicit operator Guid(ProductImageId id) => id.Value;
    public static implicit operator ProductImageId(Guid id) => new(id);
}