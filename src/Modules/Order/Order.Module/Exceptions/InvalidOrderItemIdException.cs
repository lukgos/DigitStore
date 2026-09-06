using Shared.Abstractions.Exceptions;

namespace Order.Module.Exceptions;

public sealed class InvalidOrderItemIdException() : DigitStoreException("OrderItemId is invalid.");