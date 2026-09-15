using Account.Module.Abstractions;
using Account.Module.DAL.Repositories;
using Account.Module.Entities;
using Account.Module.ValueObjects;
using Shared.Abstractions.ValueObjects;

namespace Account.Module.DAL;

public class AccountSeeder(IUserRepository userRepository, IPasswordManager passwordManager)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var adminEmail = new Email("admin@example.com");
        var userEmail = new Email("user@example.com");

        if (!await userRepository.ExistsByEmailAsync(adminEmail, cancellationToken))
        {
            var adminPassword = new PasswordHash(passwordManager.HashPassword("password"));
            var adminId = new UserId(Guid.NewGuid());

            var admin = User.Create(adminId, adminEmail, adminPassword, Role.Admin);

            await userRepository.AddAsync(admin, cancellationToken);
        }

        if (!await userRepository.ExistsByEmailAsync(userEmail, cancellationToken))
        {
            var userPassword = new PasswordHash(passwordManager.HashPassword("password"));
            var userId = new UserId(Guid.NewGuid());

            var user = User.Create(userId, userEmail, userPassword, Role.User);

            await userRepository.AddAsync(user, cancellationToken);
        }
    }
}