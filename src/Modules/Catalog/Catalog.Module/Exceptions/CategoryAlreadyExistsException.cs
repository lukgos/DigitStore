using Shared.Abstractions.Exceptions;

namespace Catalog.Module.Exceptions;

public sealed class CategoryAlreadyExistsException : DigitStoreException
{
    public CategoryAlreadyExistsException(string name) : base($"Category with name '{name}' already exists.")
    {
    }
}