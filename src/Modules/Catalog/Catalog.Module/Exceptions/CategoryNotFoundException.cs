using Shared.Abstractions.Exceptions;

namespace Catalog.Module.Exceptions;

public class CategoryNotFoundException : DigitStoreException
{
    public CategoryNotFoundException(Guid id) : base($"Category with ID {id} not found.")
    {
        
    }
}