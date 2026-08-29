namespace Search.Module.DTOs;

public record ProductSearchDto(
    Guid Id,
    string? Name,
    string? Description,
    decimal Price,
    Guid? CategoryId,
    string? CategoryName,
    IEnumerable<string> Tags,
    Dictionary<string, string> Attributes,
    string? PrimaryImageUrl);