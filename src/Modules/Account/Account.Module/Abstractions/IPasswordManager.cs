namespace Account.Module.Abstractions;

public interface IPasswordManager
{
    string HashPassword(string password);
    bool ValidatePassword(string password, string hashedPassword);
}