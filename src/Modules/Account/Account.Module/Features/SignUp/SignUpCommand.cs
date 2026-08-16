using Shared.Abstractions.CQRS;

namespace Account.Module.Features.SignUp;

public record SignUpCommand(Guid Id, string Email, string Password) : ICommand;