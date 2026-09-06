using Shared.Abstractions.Exceptions;

namespace Order.Module.Exceptions;

public sealed class ProductNotFoundException(Guid productId) : DigitStoreException($"Product with Id: {productId} was not found.");