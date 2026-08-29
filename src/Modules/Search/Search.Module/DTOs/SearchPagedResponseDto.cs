namespace Search.Module.DTOs;

public record SearchPagedResponseDto(
    IEnumerable<ProductSearchDto> Items,
    long TotalFound,
    int CurrentPage,
    int TotalPages
);