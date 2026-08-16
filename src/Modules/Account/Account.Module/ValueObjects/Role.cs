using Account.Module.Exceptions;

namespace Account.Module.ValueObjects;

public record Role
{
    public const string User = "user";
    public const string Admin = "admin";

    public string Value { get; }
    
    public Role(string value)
    {
        if (value.ToLower() != User && value.ToLower() != Admin)
        {
            throw new InvalidRoleException(); 
        }

        Value = value.ToLower();
    }
    
    public static implicit operator string(Role role) => role.Value;
    public static implicit operator Role(string value) => new(value);
}