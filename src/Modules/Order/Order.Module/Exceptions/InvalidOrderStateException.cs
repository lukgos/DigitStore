using Shared.Abstractions.Exceptions;

namespace Order.Module.Exceptions;


public sealed class InvalidOrderStateException() : DigitStoreException("OrderState is invalid.");