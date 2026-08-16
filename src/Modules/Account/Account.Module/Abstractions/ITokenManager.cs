using System.Security.Claims;
using Account.Module.Entities;

namespace Account.Module.Abstractions;

public interface ITokenManager
{
    string GenerateAccessToken(User user);
}