namespace Shared.Abstractions.Common;

public abstract class Entity<TId> : IEntity<TId>
{
    public required TId Id { get; init; }
}