using Account.Module.Abstractions;
using Account.Module.DAL.Repositories;
using Account.Module.Exceptions;
using Account.Module.ValueObjects;
using Shared.Abstractions.CQRS;

namespace Account.Module.Features.SignIn;

public sealed class SignInCommandHandler(
    IUserRepository userRepository,
    IPasswordManager passwordManager,
    ITokenManager tokenManager,
    ITokenStorage tokenStorage) : ICommandHandler<SignInCommand>
{
    public async Task HandleAsync(
        SignInCommand command,
        CancellationToken cancellationToken)
    {
        var email = new Email(command.Email);

        var user = await userRepository.GetByEmailAsync(email, cancellationToken);

        if (user is null)
        {
            throw new InvalidCredentialsException();
        }

        var isPasswordValid = passwordManager.ValidatePassword(command.Password, user.PasswordHash.Value);

        if (!isPasswordValid)
        {
            throw new InvalidCredentialsException();
        }

        var accessToken = tokenManager.GenerateAccessToken(user);

        tokenStorage.Set(accessToken);
    }
}