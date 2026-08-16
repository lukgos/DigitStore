using Shared.Abstractions.Exceptions;

namespace Account.Module.Exceptions;

public sealed class InvalidEmailException(string email) : DigitStoreException($"Invalid email: {email}")
{
    public string Email { get; } = email;
}