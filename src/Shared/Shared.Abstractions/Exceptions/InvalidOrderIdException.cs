namespace Shared.Abstractions.Exceptions;

public sealed class InvalidOrderIdException() : DigitStoreException("OrderId is invalid.");