using System.Security.Claims;

namespace Shared.Abstractions.Auth;

public interface IUserContext
{
    ClaimsPrincipal Principal { get; }
    Guid? UserId { get; }
    string? UserRole { get; }
    bool IsAuthenticated { get; }
}