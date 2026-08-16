using Shared.Abstractions.Exceptions;

namespace Shared.Abstractions.ValueObjects;

public record UserId
{
    public Guid Value { get; }

    public UserId(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new InvalidUserIdException();
        }
        
        Value = userId;
    }
    
    public static implicit operator Guid(UserId userId) => userId.Value;
    public static implicit operator UserId(Guid userId) => new(userId);
}