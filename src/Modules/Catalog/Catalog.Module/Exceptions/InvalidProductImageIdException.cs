using Shared.Abstractions.Exceptions;

namespace Catalog.Module.Exceptions;

public sealed class InvalidProductImageIdException() : DigitStoreException("ProductImageId is invalid.");