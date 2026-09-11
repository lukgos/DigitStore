namespace Search.Module.DTOs;

public record ProductSearchDto(
    Guid Id,
    string? Name,
    string? Description,
    decimal Price,
    Guid? CategoryId,
    string? CategoryName,
    IEnumerable<string> Tags,
    IReadOnlyDictionary<string, string> Attributes,
    string? PrimaryImageUrl);