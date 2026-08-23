namespace Catalog.Module.DTOs;

public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    Guid? CategoryId,
    IEnumerable<string> Tags,
    IEnumerable<ProductImageDto> Images,
    IReadOnlyDictionary<string, string> Attributes,
    int Version
);