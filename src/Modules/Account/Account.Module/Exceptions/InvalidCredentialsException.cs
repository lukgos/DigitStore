using Shared.Abstractions.Exceptions;

namespace Account.Module.Exceptions;

public sealed class InvalidCredentialsException () : DigitStoreException("Sign In Credentials are invalid.");