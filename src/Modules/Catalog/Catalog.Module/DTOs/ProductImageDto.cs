namespace Catalog.Module.DTOs;

public record ProductImageDto(Guid Id, string Url, string AltText, bool IsPrimary);