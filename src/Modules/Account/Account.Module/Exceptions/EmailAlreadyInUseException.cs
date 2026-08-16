using Shared.Abstractions.Exceptions;

namespace Account.Module.Exceptions;

public sealed class EmailAlreadyInUseException(string email) : DigitStoreException($"Email: '{email}' is already in use.")
{
    public string Email { get; } = email;
}