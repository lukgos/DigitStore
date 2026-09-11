namespace Payment.Contracts.Events;

public record PaymentCompleted(Guid OrderId);