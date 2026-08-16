using Account.Module.Abstractions;

namespace Account.Module.Services;

public sealed class TokenStorage : ITokenStorage
{
    private string? _token;

    public void Set(string token)
    {
        _token = token;
    }

    public string Get()
    {
        return _token ?? throw new InvalidOperationException("Token not set.");
    }
}
