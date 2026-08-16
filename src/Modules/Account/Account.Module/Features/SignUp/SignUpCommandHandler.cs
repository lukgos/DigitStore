using Account.Module.Abstractions;
using Account.Module.DAL.Repositories;
using Account.Module.Entities;
using Account.Module.Exceptions;
using Account.Module.ValueObjects;
using Shared.Abstractions.CQRS;
using Shared.Abstractions.ValueObjects;

namespace Account.Module.Features.SignUp;

public sealed class SignUpCommandHandler(IUserRepository userRepository, IPasswordManager passwordManager) : ICommandHandler<SignUpCommand>
{
    public async Task HandleAsync(SignUpCommand command, CancellationToken cancellationToken = default)
    {
        var email = new Email(command.Email);

        if (await userRepository.ExistsByEmailAsync(email, cancellationToken))
        {
            throw new EmailAlreadyInUseException(command.Email);
        }

        var passwordHash = new PasswordHash(passwordManager.HashPassword(command.Password));
        var userId = new UserId(command.Id);

        var user = User.Create(userId, email, passwordHash, Role.User);

        await userRepository.AddAsync(user, cancellationToken);
    }
}