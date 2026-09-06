using Shared.Abstractions.Exceptions;

namespace Order.Module.Exceptions;

public sealed class InvalidQuantityException() : DigitStoreException("Quantity is invalid.");