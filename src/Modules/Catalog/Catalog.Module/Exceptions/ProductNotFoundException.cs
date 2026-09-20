using Shared.Abstractions.Exceptions;

namespace Catalog.Module.Exceptions;

public sealed class ProductNotFoundException(Guid id) : DigitStoreException($"Product with ID '{id}' was not found.");