namespace Shared.Abstractions.Common;

public interface IAuditableEntity
{
    DateTimeOffset CreatedAt { get; set; }
    Guid? CreatedBy { get; set; }
    DateTimeOffset? LastModifiedAt { get; set; }
    Guid? LastModifiedBy { get; set; }
    bool IsDeleted { get; set; }
    DateTimeOffset? DeletedAt { get; set; }
    Guid? DeletedBy { get; set; }
    int Version { get; set; }
}