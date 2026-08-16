using Shared.Abstractions.Exceptions;

namespace Account.Module.Exceptions;

public sealed class InvalidPasswordException() : DigitStoreException("Password is invalid.");