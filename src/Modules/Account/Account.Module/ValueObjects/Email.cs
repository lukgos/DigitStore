using System.Net.Mail;
using Account.Module.Exceptions;

namespace Account.Module.ValueObjects;

public record Email
{
    public string Value { get; }
    
    public Email(string email)
    {
        if (!IsEmailValid(email))
        {
            throw new InvalidEmailException(email);
        }
        Value = email;
    }
    
    public static implicit operator string(Email email) => email.Value;
    public static implicit operator Email(string value) => new(value);
    
    private static bool IsEmailValid(string value)
    {
        try
        {
            var mailAddress = new MailAddress(value);
            return mailAddress.Address == value;
        }
        catch
        {
            return false;
        }
    }
}