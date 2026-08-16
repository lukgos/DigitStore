using Account.Module.Entities;
using Shared.Abstractions.CQRS;

namespace Account.Module.Features.SignIn;

public record SignInCommand(string Email, string Password) : ICommand;