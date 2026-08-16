namespace Account.Module.Abstractions;

public interface ITokenStorage
{
    void Set(string token);
    string Get();
}