using Search.Module.DTOs;
using Shared.Abstractions.CQRS;

namespace Search.Module.Features.SearchProducts;

public record SearchProductsQuery(string? Phrase, Guid? CategoryId, decimal? MinPrice, decimal? MaxPrice, int Page = 1, int PageSize = 10) : IQuery<SearchPagedResponseDto>;