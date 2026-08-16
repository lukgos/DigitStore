using Account.Module.Abstractions;
using Account.Module.Entities;
using Microsoft.AspNetCore.Identity;

namespace Account.Module.Services;

internal sealed class PasswordManager(IPasswordHasher<User> passwordHasher) : IPasswordManager
{
    public string HashPassword(string password)
    {
        var hashedPassword = passwordHasher.HashPassword(default!, password);
        return hashedPassword;
    }

    public bool ValidatePassword(string password, string hashedPassword)
    {
        if (passwordHasher.VerifyHashedPassword(default!, hashedPassword, password) == PasswordVerificationResult.Success)
            return true;
        return false;
    }
}