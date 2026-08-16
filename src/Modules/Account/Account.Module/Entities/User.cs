using Account.Module.Abstractions;
using Account.Module.Exceptions;
using Account.Module.ValueObjects;
using Shared.Abstractions.Common;
using Shared.Abstractions.ValueObjects;

namespace Account.Module.Entities;

public sealed class User : AuditableEntity<UserId>
{
    public Email Email { get; private set; }
    public Role Role { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public PasswordHash PasswordHash { get; private set; }
    

    private User()
    {
    }

    private User(UserId id, Email email, PasswordHash passwordHash, Role role)
    {
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }

    public static User Create(UserId id, Email email, PasswordHash passwordHash, Role role)
    {
        var user = new User(id, email, passwordHash, role)
        {
            Id = id
        };
        
        return user;
    }

    public void UpdatePassword(string password, string oldPassword, IPasswordManager passwordManager)
    {
        if (!passwordManager.ValidatePassword(oldPassword, PasswordHash.Value))
        {
            throw new InvalidPasswordException();
        }
        
        PasswordHash = passwordManager.HashPassword(password);
    }

    public void UpdateEmail(Email email)
    {
        Email = email;
    }

    public void ConfirmEmail()
    {
        EmailConfirmed = true;
    }
    
    public void ChangeRole(Role newRole)
    {
        Role = newRole;
    }
}