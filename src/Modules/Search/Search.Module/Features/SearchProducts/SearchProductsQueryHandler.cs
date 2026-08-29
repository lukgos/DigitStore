using OpenSearch.Client;
using Search.Module.Documents;
using Search.Module.DTOs;
using Shared.Abstractions.CQRS;

namespace Search.Module.Features.SearchProducts;

public sealed class SearchProductsQueryHandler(IOpenSearchClient client) : IQueryHandler<SearchProductsQuery, SearchPagedResponseDto>
{
    public async Task<SearchPagedResponseDto> HandleAsync(SearchProductsQuery query, CancellationToken ct)
    {
        var queryBuilder = new QueryContainerDescriptor<ProductDocument>();
        var conditions = new List<QueryContainer>();

        if (!string.IsNullOrWhiteSpace(query.Phrase))
        {
            conditions.Add(queryBuilder.MultiMatch(multiMatch => multiMatch
                .Fields(fieldDescriptor => fieldDescriptor
                    .Field(p => p.Name, boost: 2.0) 
                    .Field(p => p.Description)
                    .Field(p => p.Tags)
                    .Field("attributes.*")
                )
                .Query(query.Phrase)
                .Fuzziness(Fuzziness.Auto)
                .Lenient(true)
            ));
        }

        if (query.CategoryId.HasValue)
        {
            conditions.Add(queryBuilder.Term(t => t
                .Field(p => p.CategoryId)
                .Value(query.CategoryId.Value.ToString())
            ));
        }

        if (query.MinPrice.HasValue || query.MaxPrice.HasValue)
        {
            conditions.Add(queryBuilder.Range(r => r
                .Field(p => p.Price)
                .GreaterThanOrEquals((double?)query.MinPrice)
                .LessThanOrEquals((double?)query.MaxPrice)
            ));
        }

        var queryContainer = conditions.Count == 0 ? queryBuilder.MatchAll() : queryBuilder.Bool(b => b.Must(conditions.ToArray()));

        var from = (query.Page - 1) * query.PageSize;
        var response = await client.SearchAsync<ProductDocument>(searchDescriptor => searchDescriptor
            .Index("products")
            .From(from)
            .Size(query.PageSize)
            .Query(descriptor => queryContainer), ct);

        if (!response.IsValid)
        {
            throw new InvalidOperationException($"OpenSearch Query Failed: {response.DebugInformation}");
        }

        var items = response.Documents.Select(d => new ProductSearchDto(
            d.Id, d.Name, d.Description, d.Price, d.CategoryId, d.CategoryName, d.Tags, d.Attributes, d.PrimaryImageUrl
        )).ToList();

        var totalPages = (int)Math.Ceiling(response.Total / (double)query.PageSize);

        return new SearchPagedResponseDto(items, response.Total, query.Page, totalPages);
    }
}