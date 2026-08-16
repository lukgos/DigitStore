using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Shared.Abstractions.Auth;

namespace Shared.Infrastructure.Auth;

internal sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public ClaimsPrincipal Principal => httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();

    public Guid? UserId
    {
        get
        {
            var claim = Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(claim, out var userId) ? userId : null;
        }
    }

    public string? UserRole => Principal.FindFirst(ClaimTypes.Role)?.Value;

    public bool IsAuthenticated => Principal.Identity?.IsAuthenticated ?? false;
}