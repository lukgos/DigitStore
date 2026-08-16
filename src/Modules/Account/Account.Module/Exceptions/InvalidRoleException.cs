using Shared.Abstractions.Exceptions;

namespace Account.Module.Exceptions;

public sealed class InvalidRoleException() : DigitStoreException("Role is invalid.");