using Shared.Abstractions.Exceptions;

namespace Catalog.Module.Exceptions;

public class CannotDeleteDefaultCategoryException : DigitStoreException
{
    public CannotDeleteDefaultCategoryException() : base("Cannot delete the default category.")
    {
        
    }
}