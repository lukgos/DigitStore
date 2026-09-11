namespace Account.Contracts.Events;

public record AccountCreated(Guid UserId, string Email);