using Shared.Abstractions.Exceptions;

namespace Catalog.Module.Exceptions;


public sealed class InvalidProductPriceHistoryIdException() : DigitStoreException("ProductPriceHistoryId is invalid.");