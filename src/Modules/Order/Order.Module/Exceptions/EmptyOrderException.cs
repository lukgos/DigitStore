using Shared.Abstractions.Exceptions;

namespace Order.Module.Exceptions;

public sealed class EmptyOrderException() : DigitStoreException("Order cannot be empty.");